using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Step))]
    public class StepDto : EntityDto<long>
    {
        public StepType Type { get; set; }
        public bool IsActive { get; set; }
        public ICollection<StepContentDto> Content { get; set; }
        public int Duration { get; set; }
        public Block Block { get; set; }
        public int Index { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
    }
}
