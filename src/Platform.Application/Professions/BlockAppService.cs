using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.Dtos.Block;
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
        public BlockAppService(IRepository<Block, long> blockRepository, IRepository<BlockTranslations, long> translationRepository)
        {
            repository = blockRepository;
            this.translationRepository = translationRepository;
        }

        //public Task AddStep(AddStepDto input)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task AddTranslation(AddBlockTranslationDto input, long id)
        {
            var translation = ObjectMapper.Map(input, new BlockTranslations());
            var block = await repository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            block.Translations.Add(translation);
        }

        //public async Task<BlockReplyOkDto> CreateBlock(CreateBlockDto input)
        //{
        //    var block = ObjectMapper.Map<Block>(input);
        //    var newid = await repository.InsertAndGetIdAsync(block);
        //    return new BlockReplyOkDto { id = newid, message = "created" };
        //}

        //public async Task<BlockReplyOkDto> CreateCopy(long id)
        //{
        //    var block= await repository.GetAllIncluding(p => p.Translations)
        //       .FirstOrDefaultAsync(p => p.Id == id);
        //    var newblockdto = ObjectMapper.Map(block, new CreateBlockDto());
        //    var newblock = ObjectMapper.Map(newblockdto, new Block());
        //    var newid = await repository.InsertAndGetIdAsync(newblock);
        //    return new BlockReplyOkDto { id = newid, message = "created" };
        //}

        public async Task<BlockReplyOkDto> DeleteBlock(long id)
        {
            var block = await repository.GetAllIncluding(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
            block.IsDeleted = true;
            block.IsActive = false;
            var res = await repository.InsertOrUpdateAndGetIdAsync(block);
            return new BlockReplyOkDto { id = res, message = "deleted" };
        }

        public async Task DeleteTranslation(DeleteBlockTranslationDto input)
        {
            var ts = await repository.FirstOrDefaultAsync(p => p.Id == input.id);
            ts.IsActive = false;
            ts.IsDeleted = true;
            await repository.InsertOrUpdateAsync(ts);
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

        //public Task RemoveStep(RemoveStepDto input)
        //{
        //    throw new NotImplementedException();
        //}

        public  async Task<BlockReplyOkDto> UpdateBlock(UpdateBlockDto input)
        {
            var block = await repository.GetAllIncluding(p => p.Translations)
              .FirstOrDefaultAsync(p => p.Id == input.Id);
            block.Translations.Clear();
            var upd = ObjectMapper.Map(input, block);
            var res = await repository.InsertOrUpdateAndGetIdAsync(upd);
            return new BlockReplyOkDto { id = res, message = "updated" };
        }

        public async Task UpdateTranslation(UpdateBlockTranslationDto input)
        {
            var ts = ObjectMapper.Map<BlockTranslations>(input);
            await translationRepository.InsertOrUpdateAndGetIdAsync(ts);
        }
    }
}
