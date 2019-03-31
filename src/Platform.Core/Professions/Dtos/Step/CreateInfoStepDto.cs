using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos.Step
{

    [AutoMap(typeof(StepInfo))]
    public class CreateInfoStepDto
    {
        [DataType(DataType.DateTime)]
        public int Duration { get; set; }
        public int Index { get; set; }
        public ICollection<CreateStepTranslationDto> Translations { get; set; }
    }
}
