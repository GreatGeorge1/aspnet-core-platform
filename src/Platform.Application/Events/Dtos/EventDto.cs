﻿using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(Event))]
    public class EventDto : EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }     
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long ProfessionId { get; set; }
    }
}
