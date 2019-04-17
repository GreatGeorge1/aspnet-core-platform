using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(Event))]
    public class EventCreateDto:EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<EventTranslationDto> Translations { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        //public ICollection<EventProfession> EventProfessions { get; set; }
    }
}
