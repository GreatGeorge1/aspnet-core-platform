using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Answer
{
    [AutoMap(typeof(Professions.Answer))]
    public class UpdateAnswerDto:CreateAnswerDto
    {
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public new ICollection<UpdateAnswerTranslationDto> Translations { get; set; }
    }
}
