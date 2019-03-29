using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(ProfessionTranslations))]
    public class ProfessionTranslationsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
        public string Language { get; set; }
    }
}