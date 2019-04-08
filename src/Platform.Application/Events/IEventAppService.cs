using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Events
{
    public interface IEventAppService : IAsyncCrudAppService<EventDto, long, PagedResultDto<Event>, CreateEventDto, UpdateEventDto>
    {
        Task AddTranslation(AddEventTranslationDto input, long id);
        Task UpdateTranslation(EventTranslationDto input);
        Task RemoveTranslation(RemoveEventTranslationDto input);
        Task AddProfession(AddEventProfessionDto input);
        Task RemoveProfession(RemoveEventProfessionDto input);
        Task<EventDto> CreateCopy(long id);
    }
}
