using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{

    [AutoMap(typeof(Step))]
    public class StepCreateDto:EntityDto<long>
    {
        [Required]
        public StepType Type { get; set; }
        public bool IsActive { get; set; }
        public StepContentDto Content { get; set; }
        public int Duration { get; set; }
        public int Index { get; set; }
    }
}
