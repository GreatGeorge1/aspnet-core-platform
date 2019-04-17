using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IBlockAppService: IAsyncCrudAppService<BlockDto, long, PagedResultDto<Block>, BlockCreateDto, BlockUpdateDto>
    {
        Task CreateInfoStep(StepInfoCreateDto input, long id);
        Task CreateTestStep(StepTestCreateDto input, long id);
        Task DeleteStep(StepDeleteDto input);
        Task CreateTranslation(BlockTranslationDto input, long id);
        Task UpdateTranslation(BlockTranslationDto input);
        Task DeleteTranslation(BlockTranslationDeleteDto input);
    }
}
