using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IStepTestAppService : IAsyncCrudAppService<StepTestDto, long, PagedResultDto<StepTest>, StepTestCreateDto, StepTestUpdateDto>
    {
        Task CreateTranslation(StepTranslationDto input, long id);
        Task UpdateTranslation(StepTranslationDto input);
        Task DeleteTranslation(StepTranslationDeleteDto input);
        Task CreateAnswer(AnswerCreateDto input, long id);
        Task DeleteAnswer(AnswerDeleteDto input);
    }
}
