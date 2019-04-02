using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepInfo))]
    public class UpdateInfoStepDto:CreateInfoStepDto
    {
        public long Id;
        public bool IsActive;
        public new ICollection<UpdateStepTranslationDto> Translations { get; set; }
    }
}
