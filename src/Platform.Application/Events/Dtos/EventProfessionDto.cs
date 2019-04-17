using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Dtos;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(EventProfession))]
    public class EventProfessionDto:EntityDto<long>
    {
        public long EventId { get; set; }
        //public ProfessionDto Profession { get; set; }
        public long ProfessionId { get; set; }
    }

    
}
