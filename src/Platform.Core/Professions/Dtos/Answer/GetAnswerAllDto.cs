using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Answer
{
    [AutoMap(typeof(Professions.Answer))]
    public class GetAnswerAllDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsCorrect { get; set; }
        public ICollection<UpdateAnswerTranslationDto> Translations { get; set; }
    }
}
