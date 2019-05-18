using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Dynamic;
using System.Collections.Generic;
using Abp.BackgroundJobs;
using Platform.Background;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FluentFTP;
using System.Text.Encodings.Web;
using Unidecode.NET;

namespace Platform.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize(AuthenticationSchemes = "JwtBearer")]
    public class FileController : AbpController
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly FileDomainService fileService;

        public FileController(IBackgroundJobManager backgroundJobManager, FileDomainService fileService)
        {
            _backgroundJobManager = backgroundJobManager ?? throw new ArgumentNullException(nameof(backgroundJobManager));
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        public async Task<IActionResult> Download(string ftpurl)
        {
            if (ftpurl == null)
                return Content("filename not present");

            var tempPath =Path.GetTempPath();
            var ext = Path.GetExtension(ftpurl);
            var filename = Path.GetFileNameWithoutExtension(ftpurl);
            filename.Normalize();
            Random _rdm = new Random();
            var newpath = Path.Combine(tempPath, $"{_rdm.Next(1000, 9999)}_{filename}", ext);
          
            await fileService.client.ConnectAsync();
            await fileService.client.DownloadFileAsync(newpath, ftpurl);
            await fileService.client.DisconnectAsync();

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Headers.Append("Content-Disposition", "filename=" + newpath);
            await Response.SendFileAsync(newpath);
            System.IO.File.Delete(newpath);
            return Ok();
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        [Authorize(AuthenticationSchemes = "JwtBearer")]
        [HttpPost]
        [DisableFormValueModelBinding]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public async Task<IActionResult> UploadStream(FileUploadDto fix)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;
            string fileName = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        fileName = Path.GetFileName(contentDisposition.FileName.ToString().Unidecode());
                        targetFilePath = Path.GetTempFileName();
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);

                            Logger.Debug($"Copied the uploaded file '{targetFilePath}'");
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.Value, value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            var input = new FileUploadDto();
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);

            var bindingSuccessful = await TryUpdateModelAsync(input, prefix: "",
                valueProvider: formValueProvider);
            if (!bindingSuccessful)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            dynamic uploadedData = new ExpandoObject();
            uploadedData.FilePath = targetFilePath;

            _ = await _backgroundJobManager.EnqueueAsync<UploadFileToFtpJob, UploadFileToFtpArgs>(
            new UploadFileToFtpArgs
            {
                FileName = fileName,
                ParentType = input.ParentType,
                TempFilePath = targetFilePath,
                ParentId = input.ParentId,
                UserId = AbpSession.UserId ?? 0
            });

            return Json(uploadedData);
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }

    public class FileUploadDto
    {
        [Required]
        [EnumDataType(typeof(ParentType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParentType ParentType { get; set; }
        [Required]
        public long ParentId { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var factories = context.ValueProviderFactories;
            factories.RemoveType<FormValueProviderFactory>();
            factories.RemoveType<JQueryFormValueProviderFactory>();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
