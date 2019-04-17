using System.Collections.Generic;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Answer
{
    [AutoMap(typeof(Professions.Answer))]
    public class CreateAnswerDto
    {
        public bool IsCorrect { get; set; }
        public ICollection<CreateAnswerTranslationDto> Translations { get; set; }
    }
}
