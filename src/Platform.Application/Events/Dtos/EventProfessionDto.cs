using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(EventProfession))]
    public class EventProfessionDto:EntityDto<long>
    {
        public long EventId { get; set; }
        public GetProfessionAllDto Profession { get; set; }
        public long ProfessionId { get; set; }
    }
}
