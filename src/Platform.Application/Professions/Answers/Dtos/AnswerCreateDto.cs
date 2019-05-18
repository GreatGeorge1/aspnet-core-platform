using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Platform.Professions.Answer))]
    public class AnswerCreateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public AnswerContentDto Content { get; set; }
        public bool IsCorrect { get; set; }
    }

}
