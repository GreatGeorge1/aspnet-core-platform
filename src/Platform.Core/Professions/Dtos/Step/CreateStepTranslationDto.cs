using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepTranslations))]
    public class CreateStepTranslationDto
    {
        public string Language { get; set; }
        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}
