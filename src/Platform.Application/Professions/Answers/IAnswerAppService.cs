using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IAnswerAppService: IAsyncCrudAppService<AnswerDto, long, PagedResultDto<Answer>, AnswerCreateDto, AnswerUpdateDto>
    {
        Task CreateTranslation(AnswerTranslationDto input, long id);
        Task UpdateTranslation(AnswerTranslationDto input);
        Task DeleteTranslation(AnswerTranslationDeleteDto input);
    }
}
