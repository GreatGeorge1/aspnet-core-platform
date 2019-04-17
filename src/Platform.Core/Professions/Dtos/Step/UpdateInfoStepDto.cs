using System.Collections.Generic;
using Abp.AutoMapper;

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
