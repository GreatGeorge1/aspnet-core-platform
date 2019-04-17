using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(StepInfo))]
    public class StepInfoDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<StepTranslationDto> Translations { get; set; }
        public int Duration { get; set; }
        public Block Block { get; set; }
        public int Index { get; set; }
    }
}
