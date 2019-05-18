using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using FluentFTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Platform.Files;
using Platform.Professions;
using Platform.Professions.Blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Background
{
    public class UploadFileToFtpJob : BackgroundJob<UploadFileToFtpArgs>, ITransientDependency
    {
        private readonly IRepository<Profession, long> professionRepository;
        private readonly IRepository<Block, long> blockRepository;
        private readonly IRepository<Step, long> stepRepository;
        private readonly IRepository<Answer, long> answerRepository;
        private readonly IRepository<SingleFile, long> fileRepository;
        private readonly FileDomainService fileService;
        public IEventBus EventBus { get; set; }

        public UploadFileToFtpJob(IRepository<Profession, long> professionRepository, 
            IRepository<Block, long> blockRepository, 
            IRepository<Step, long> stepRepository, 
            IRepository<Answer, long> answerRepository,
            IRepository<SingleFile, long> fileRepository,
            FileDomainService fileService)
        {
            this.professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            this.blockRepository = blockRepository ?? throw new ArgumentNullException(nameof(blockRepository));
            this.stepRepository = stepRepository ?? throw new ArgumentNullException(nameof(stepRepository));
            this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            EventBus = NullEventBus.Instance;
        }

        [UnitOfWork]
        public override void Execute(UploadFileToFtpArgs args)
        {
            var url= fileService.UploadToFtp(args);
            //url = $"ftp://{ftpurl}{url}";
            File.Delete(args.TempFilePath);
            switch (args.ParentType)
            {
                case ParentType.Profession:
                    Process<Profession, ProfessionContent, long>(url, args.ParentId, professionRepository);
                    break;
                case ParentType.Block:
                    Process<Block, BlockContent, long>(url, args.ParentId, blockRepository);
                    break;
                case ParentType.Step:
                    Process<Step, StepContent, long>(url, args.ParentId, stepRepository);
                    break;
                case ParentType.Answer:
                    Process<Answer, AnswerContent, long>(url, args.ParentId, answerRepository);
                    break;
                case ParentType.None:
                    Process(url);
                    break;
                default:
                    throw new ArgumentException($"{nameof(args.ParentType)}");
                    break;
            }
            EventBus.Trigger(this, new FileUploadedEventData
            {
                UserId = args.UserId,
                ParentId = args.ParentId,
                FileName = args.FileName,
                ParentType = args.ParentType
            });

        }

        [UnitOfWork]
        private void Process<TEntity, TContent, TKey>(string url, long ParentId, IRepository<TEntity, TKey> repository)
            where TEntity: Entity<TKey>, IHasContent<TEntity, TContent, TKey>
            where TContent:GenericContent<TEntity,TKey>
            where TKey : IEquatable<TKey>
        {
            var parent = repository.GetAllIncluding(p => p.Content).FirstOrDefault(p => p.Id.Equals(ParentId));
            var content = parent.Content;
            if (content.FileUrls == null)
            {
                content.FileUrls = new List<string>();
            }
            if (content.FileUrls.Any())
            {
                foreach (var item in content.FileUrls.ToList())
                {
                    if (item.Equals(url))
                    {
                        content.FileUrls.Remove(url);
                    }
                }
            }
            content.FileUrls.Add(url);
        }

        [UnitOfWork]
        private void Process(string url)
        {
            var file= fileRepository.FirstOrDefault(f=>f.Path==url);
            if (file == null)
            {
                fileRepository.Insert(new SingleFile
                {
                    Path = url
                });
            }
        }

    }

    [Serializable]
    public class UploadFileToFtpArgs
    {
        public string TempFilePath { get; set; }
        public string FileName { get; set; }
        //public string Mime { get; set; }
        [EnumDataType(typeof(ParentType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParentType ParentType { get; set; }
        public long ParentId { get; set; }
        public long UserId { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ParentType
    {
        Profession, 
        Block,
        Step,
        Answer,
        None
    }
}
