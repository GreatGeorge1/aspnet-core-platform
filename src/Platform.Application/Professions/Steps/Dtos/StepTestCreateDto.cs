using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(StepTest))]
    public class StepTestCreateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<StepTranslationDto> Translations { get; set; }
        public int Duration { get; set; }
        public int Index { get; set; }
    }


}
