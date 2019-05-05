using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Professions.Dtos;

namespace Platform.Professions
{
    public interface IAnswerAppService: IAsyncCrudAppService<AnswerDto, long, PagedResultDto<Answer>, AnswerCreateDto, AnswerUpdateDto>
    {
        Task<AnswerContentDto> UpdateContent(AnswerContentDto input);
    }
}
