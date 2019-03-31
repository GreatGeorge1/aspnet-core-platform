using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos.Block;
using Platform.Professions.Dtos.Step;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public class BlockAppService : ApplicationService, IBlockAppService
    {
        private readonly IRepository<Block, long> repository;
        private readonly IRepository<BlockTranslations, long> translationRepository;
        private readonly IRepository<StepInfo, long> stepInfoRepository;
        public BlockAppService(IRepository<Block, long> blockRepository,
            IRepository<BlockTranslations, long> translationRepository,
             IRepository<StepInfo, long> stepInfoRepository)
        {
            repository = blockRepository;
            this.translationRepository = translationRepository;
            this.stepInfoRepository = stepInfoRepository;
        }   

        public async Task<GetBlockDto> GetBlock(long id)
        {
            var prof = await repository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(prof, new GetBlockDto());
            return res;
        }

        public async Task<GetBlockAllDto> GetBlockAll(long id)
        {
            var prof = await repository.GetAllIncluding(p => p.Translations)
             .FirstOrDefaultAsync(p => p.Id == id);
            var res = ObjectMapper.Map(prof, new GetBlockAllDto());
            return res;
        }

        public async Task<BlockReplyOkDto> UpdateBlock(UpdateBlockDto input)
        {
            var block = await repository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.Id);
            block.Translations.Clear();
            var upd = ObjectMapper.Map(input, block);
            var res = await repository.InsertOrUpdateAndGetIdAsync(upd);
            return new BlockReplyOkDto { id = res, message = "updated" };
        }

        public async Task<BlockReplyOkDto> DeleteBlock(long id)
        {
            var block = await repository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            block.IsDeleted = true;
            block.IsActive = false;
            var res = await repository.InsertOrUpdateAndGetIdAsync(block);
            return new BlockReplyOkDto { id = res, message = "deleted" };
        }

        public async Task AddTranslation(AddBlockTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new BlockTranslations());
            var block = await repository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            block.Translations.Add(translation);
        }

        public async Task UpdateTranslation(UpdateBlockTranslationDto input)
        {
            var ts = ObjectMapper.Map<BlockTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }

        public async Task DeleteTranslation(DeleteBlockTranslationDto input)
        {
            var ts = await translationRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await translationRepository.InsertOrUpdateAsync(ts);
        }

        public async Task AddInfoStep(CreateInfoStepDto input, long id)
        {
            var block = await repository.FirstOrDefaultAsync(p => p.Id == id);
            var step = ObjectMapper.Map(input, new StepInfo());
            step.Block = block;
            var newid = await stepInfoRepository.InsertAndGetIdAsync(step);
           // var s= await stepInfoRepository.FirstOrDefaultAsync(p => p.Id == newid);
            //s.Block = block;
        }

        public async Task RemoveStep(RemoveStepDto input)
        {
            //steptest
            var block = await repository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.BlockId);
            var stepInfo = await stepInfoRepository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.StepId);
            block.IsActive = false;
            block.IsDeleted = true;
        }
    }
}
