using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Blocks;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public class BlockAppService : AsyncCrudAppService<Block, BlockDto, long, PagedResultDto<Block>, BlockCreateDto, BlockUpdateDto>, IBlockAppService
    {
        private readonly IRepository<Block, long> repository;
        private readonly IRepository<BlockContent, long> translationRepository;
        private readonly IRepository<Step, long> stepRepository;

        public BlockAppService(IRepository<Block, long> repository, IRepository<BlockContent, long> translationRepository, IRepository<Step, long> stepRepository)
            :base(repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            this.stepRepository = stepRepository ?? throw new ArgumentNullException(nameof(stepRepository));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<BlockDto> Create(BlockCreateDto input)
        {
            throw new NotSupportedException("Создание вне контекста профессии запрещено");
        }

        public async Task<BlockContentDto> UpdateContent(BlockContentDto input)
        {
            var ts = ObjectMapper.Map<BlockContent>(input);
            var updid = await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
            var updts = await translationRepository.FirstOrDefaultAsync(updid);
            return ObjectMapper.Map<BlockContentDto>(updts);
        }

        public async Task CreateStep(StepCreateDto input, long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Block id cannot be 0 or null");
            }
            var block = await repository.FirstOrDefaultAsync(p => p.Id == id);
            var step = ObjectMapper.Map(input, new Step());
            step.Block = block;
            var newid = await stepRepository.InsertAndGetIdAsync(step);
           // var s= await stepInfoRepository.FirstOrDefaultAsync(p => p.Id == newid);
            //s.Block = block;
        }

        public async Task DeleteStep(StepDeleteDto input)
        {
            var block = await repository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId);
            var step = await stepRepository.GetAllIncluding(p => p.Content)
              .FirstOrDefaultAsync(p => p.Id == input.StepId) ?? throw new ArgumentNullException(nameof(Step));
            step.IsActive = false;
            step.IsDeleted = true;
        }

        protected override IQueryable<Block> CreateFilteredQuery(PagedResultDto<Block> input)
        {
            return repository.GetAllIncluding(p => p.Content, p=>p.Steps).AsQueryable();
        }

        protected override async Task<Block> GetEntityByIdAsync(long id)
        {
            if (id == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var entity = await repository.GetAllIncluding(p => p.Content, p => p.Steps).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(Block), id);
            }
            return entity;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task ChangeContentVersion(long version)
        {
            throw new NotImplementedException();
        }
    }
}
