using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IStepInfoAppService : IAsyncCrudAppService<StepInfoDto, long, PagedResultDto<StepInfo>, StepInfoCreateDto, StepInfoUpdateDto>
    {
        Task CreateTranslation(StepTranslationDto input, long id);
        Task UpdateTranslation(StepTranslationDto input);
        Task DeleteTranslation(StepTranslationDeleteDto input);
    }
}
