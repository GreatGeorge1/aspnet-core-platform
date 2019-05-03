using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IStepAppService : IAsyncCrudAppService<StepDto, long, PagedResultDto<Step>, StepCreateDto, StepCreateDto>
    {
        Task<StepContentDto> UpdateContent(StepContentDto input);   
        Task CreateAnswer(AnswerCreateDto input, long id);
        Task DeleteAnswer(AnswerDeleteDto input);
    }
}
