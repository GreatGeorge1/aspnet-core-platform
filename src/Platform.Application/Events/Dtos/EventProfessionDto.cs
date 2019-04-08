using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(EventProfession))]
    public class EventProfessionDto
    {
        public long EventId { get; set; }
        public long ProfessionId { get; set; }
    }
}
