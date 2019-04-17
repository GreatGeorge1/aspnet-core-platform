using System.Collections.Generic;
using Abp.AutoMapper;

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
