using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Platform.Professions.Answer))]
    public class AnswerDto:EntityDto<long>
    {
        public bool IsActive { get; set; }
        public AnswerContentDto Content { get; set; }
        public bool IsCorrect { get; set; }
    }

}
