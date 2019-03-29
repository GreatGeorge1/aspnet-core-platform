using Platform.Professions.Dtos.Block;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public interface IBlockAppService
    {
        //Task<BlockReplyOkDto> CreateCopy(long id);
        //Task<BlockReplyOkDto> CreateBlock(CreateBlockDto input);
        Task<BlockReplyOkDto> UpdateBlock(UpdateBlockDto input);
        Task<BlockReplyOkDto> DeleteBlock(long id);
       // Task AddStep(AddStepDto input);
        //Task RemoveStep(RemoveStepDto input);
        Task AddTranslation(AddBlockTranslationDto input, long id);
        Task UpdateTranslation(UpdateBlockTranslationDto input);
        Task DeleteTranslation(DeleteBlockTranslationDto input);
        Task<GetBlockDto> GetBlock(long id);
        Task<GetBlockAllDto> GetBlockAll(long id);
    }
}
