using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(StepBase))]
    public class StepBaseDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<StepTranslationDto> Translations { get; set; }
        public int Duration { get; set; }
       // public long BlockId { get; set; }
        public int Index { get; set; }
    }
}
