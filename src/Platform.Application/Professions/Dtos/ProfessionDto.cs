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
        public ProfessionContentDto Content { get; set; }
        public PackageDto Package { get; set; }
        public EventDto Event { get; set; }
        public AuthorDto Author { get; set; }
    }
}
