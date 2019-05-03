using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class BlockAppService : AsyncCrudAppService<Block, BlockDto, long, PagedResultDto<Block>, BlockCreateDto, BlockUpdateDto>, IBlockAppService
    {
        private readonly IRepository<Block, long> repository;
        private readonly IRepository<BlockTranslations, long> translationRepository;
        private readonly IRepository<StepInfo, long> stepInfoRepository;
        private readonly IRepository<StepTest, long> stepTestRepository;
        public BlockAppService(IRepository<Block, long> blockRepository,
            IRepository<BlockTranslations, long> translationRepository,
             IRepository<StepInfo, long> stepInfoRepository,
             IRepository<StepTest, long> stepTestRepository):base(blockRepository)
        {
            repository = blockRepository;
            this.translationRepository = translationRepository;
            this.stepInfoRepository = stepInfoRepository;
            this.stepTestRepository = stepTestRepository;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<BlockDto> Create(BlockCreateDto input)
        {
            throw new NotSupportedException("Создание вне контекста профессии запрещено");
        }

        public async Task CreateTranslation(BlockTranslationDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var translation = ObjectMapper.Map(input, new BlockTranslations());
            translation.Id = 0;
            var block = await repository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            block.Translations.Add(translation);
        }

        public async Task UpdateTranslation(BlockTranslationDto input)
        {
            var ts = ObjectMapper.Map<BlockTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }

        public async Task DeleteTranslation(BlockTranslationDeleteDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task CreateInfoStep(StepInfoCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Block id cannot be 0 or null");
            }
            var block = await repository.FirstOrDefaultAsync(p => p.Id == id);
            var step = ObjectMapper.Map(input, new StepInfo());
            step.Block = block;
            var newid = await stepInfoRepository.InsertAndGetIdAsync(step);
           // var s= await stepInfoRepository.FirstOrDefaultAsync(p => p.Id == newid);
            //s.Block = block;
        }

        public async Task DeleteStep(StepDeleteDto input)
        {
            var block = await repository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId);
            var stepInfo = await stepInfoRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.StepId);
            if (stepInfo==null)
            {
                var stepTest= await stepTestRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.StepId);
                stepTest.IsActive = false;
                stepTest.IsDeleted = true;
            }
            else
            {
                stepInfo.IsActive = false;
                stepInfo.IsDeleted = true;
            }
        }

        public async Task CreateTestStep(StepTestCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Block id cannot be 0 or null");
            }
            var block = await repository.FirstOrDefaultAsync(p => p.Id == id);
            var step = ObjectMapper.Map(input, new StepTest());
            step.Block = block;
            var newid = await stepTestRepository.InsertAndGetIdAsync(step);
        }

        protected override IQueryable<Block> CreateFilteredQuery(PagedResultDto<Block> input)
        {
            return repository.GetAllIncluding(p => p.Translations, p=>p.Steps).AsQueryable();
        }

        protected override async Task<Block> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await repository.GetAllIncluding(p => p.Translations, p => p.Steps).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Block), id);
            }
            return entity;
        }
    }
}
