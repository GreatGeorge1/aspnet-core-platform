using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepTranslations))]
    public class UpdateStepTranslationDto:CreateStepTranslationDto
    {
        /// <summary>
        /// StepTranslations Id
        /// </summary>
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
