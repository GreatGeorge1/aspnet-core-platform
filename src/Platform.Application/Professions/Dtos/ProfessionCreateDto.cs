using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Platform.Events.Dtos;
using Platform.Packages.Dtos;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionCreateDto: EntityDto<long>
    {
        public bool IsActive { get; set; }
        //public int MinScore { get; set; }
        public PackageDto Package { get; set; }
        public EventDto Event { get; set; }
        public ProfessionContentDto Content { get; set; }
    }
}
