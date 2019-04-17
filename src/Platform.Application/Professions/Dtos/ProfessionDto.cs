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
        public ICollection<ProfessionTranslationDto> Translations { get; set; }
        public ICollection<PackageProfessionDto> PackageProfessions { get; set; }
        public ICollection<EventProfessionDto> EventProfessions { get; set; }
    }
}
