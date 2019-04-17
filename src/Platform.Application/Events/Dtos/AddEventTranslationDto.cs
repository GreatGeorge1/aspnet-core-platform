using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Platform.Events.Dtos
{
    [AutoMap(typeof(EventTranslations))]
    public class AddEventTranslationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
        public string Language { get; set; }
    }
    
}
