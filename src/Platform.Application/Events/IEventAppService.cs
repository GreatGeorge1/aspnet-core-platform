using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Events.Dtos;

namespace Platform.Events
{
    public interface IEventAppService : IAsyncCrudAppService<EventDto, long, PagedResultDto<Event>, EventCreateDto, EventCreateDto>
    {

    }
}
