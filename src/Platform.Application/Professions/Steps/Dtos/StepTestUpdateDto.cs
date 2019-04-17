using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(StepTest))]
    public class StepTestUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<StepTranslationDto> Translations { get; set; }
        public int Duration { get; set; }
        public int Index { get; set; }
    }


}
