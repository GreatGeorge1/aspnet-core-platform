using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Answer
{
    [AutoMap(typeof(AnswerTranslation))]
    public class UpdateAnswerTranslationDto:CreateAnswerTranslationDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
