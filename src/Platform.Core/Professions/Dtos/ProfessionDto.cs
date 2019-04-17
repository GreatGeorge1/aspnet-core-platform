using System.Collections.Generic;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionDto
    {
        public int MinScore { get; set; }
        public ICollection<ProfessionTranslationsDto> Translations { get; set; }
    }
}
