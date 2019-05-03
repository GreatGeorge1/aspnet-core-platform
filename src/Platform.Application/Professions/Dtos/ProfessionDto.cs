using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Events.Dtos;
using Platform.Packages.Dtos;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionDto:EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int MinScore { get; set; }
        public ICollection<ProfessionContentDto> Content { get; set; }
        public ICollection<PackageDto> Packages { get; set; }
        public ICollection<EventDto> Events { get; set; }
    }
}
