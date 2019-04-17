using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Events.Dtos;

namespace Platform.Events
{
    public interface IEventAppService : IAsyncCrudAppService<EventDto, long, PagedResultDto<Event>, EventCreateDto, EventUpdateDto>
    {
        Task CreateTranslation(EventTranslationDto input, long id);
        Task UpdateTranslation(EventTranslationDto input);
        Task DeleteTranslation(EventTranslationDeleteDto input);
        Task AddProfession(EventProfessionAddDto input);
        Task RemoveProfession(EventProfessionRemoveDto input);
        Task<EventDto> CreateCopy(long id);
    }
}
