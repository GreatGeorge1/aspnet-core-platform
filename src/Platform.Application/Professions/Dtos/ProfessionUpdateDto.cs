using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public int MinScore { get; set; }
        public ICollection<ProfessionTranslationDto> Translations { get; set; }
    }
}
