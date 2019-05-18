using Abp.Dependency;
using Abp.Domain.Services;
using FluentFTP;
using Platform.Background;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Platform
{
    public class FileDomainService : DomainService, ISingletonDependency
    {
        private static readonly string ftpurl = "217.182.200.111";
        public FtpClient client { get; private set; }
        public FileDomainService()
        {
            client = new FtpClient(ftpurl);
            client.Credentials = new System.Net.NetworkCredential("user622661", "g50jyvb0Hjum");
        }
      
        public string UploadToFtp(UploadFileToFtpArgs args)
        {
            using (client)
            {
                client.Connect();
                if (args.ParentType != ParentType.None)
                {
                    if (client.FileExists($"/upload/{args.ParentType}/{args.ParentId}/{args.FileName}"))
                    {
                        client.MoveFile($"/upload/{args.ParentType}/{args.ParentId}/{args.FileName}", $"/upload/{args.ParentType}/{args.ParentId}/{args.FileName}_old");
                    }
                    client.UploadFile(args.TempFilePath, $"/upload/{args.ParentType}/{args.ParentId}/{args.FileName}", createRemoteDir: true);
                    client.Disconnect();
                    return $"/upload/{args.ParentType}/{args.ParentId}/{args.FileName}";
                }
                else
                {
                    if (client.FileExists($"/upload/single/{args.FileName}"))
                    {
                        client.MoveFile($"/upload/single/{args.FileName}", $"/upload/single/{args.FileName}_old");
                    }
                    client.UploadFile(args.TempFilePath, $"/upload/single/{args.FileName}", createRemoteDir: true);
                    client.Disconnect();
                    return $"/upload/single/{args.FileName}";
                }
            }
        }


        public void DownloadFile(Stream memory, string path)
        {
            client.Connect();
            client.Download(memory, ftpurl);
            client.Disconnect();
        }

        public void DeleteFile(string path)
        {
            client.Connect();
            client.DeleteFile(path);
            client.Disconnect();
        }

    }
}
