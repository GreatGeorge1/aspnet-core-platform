using System.Threading.Tasks;
using Platform.Professions.Dtos.Block;
using Platform.Professions.Dtos.Step;

namespace Platform.Professions
{
    public interface IBlockAppService
    {
        //Task<BlockReplyOkDto> CreateCopy(long id);
        //Task<BlockReplyOkDto> CreateBlock(CreateBlockDto input);
        Task<BlockReplyOkDto> UpdateBlock(UpdateBlockDto input);
        Task<BlockReplyOkDto> DeleteBlock(long id);
        Task AddInfoStep(CreateInfoStepDto input, long id);
        Task AddTestStep(CreateTestStepDto input, long id);
        Task RemoveStep(RemoveStepDto input);
        Task AddTranslation(AddBlockTranslationDto input, long id);
        Task UpdateTranslation(UpdateBlockTranslationDto input);
        Task DeleteTranslation(DeleteBlockTranslationDto input);
        Task<GetBlockDto> GetBlock(long id);
        Task<GetBlockAllDto> GetBlockAll(long id);
    }
}
