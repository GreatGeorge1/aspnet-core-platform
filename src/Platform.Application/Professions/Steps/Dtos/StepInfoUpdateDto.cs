using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(StepInfo))]
    public class StepInfoUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        [DataType(DataType.DateTime)]
        public int Duration { get; set; }
        public int Index { get; set; }
        public ICollection<StepTranslationDto> Translations { get; set; }
    }
}
