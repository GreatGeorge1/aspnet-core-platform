using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(ProfessionTranslations))]
    public class ProfessionTranslationsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }
}