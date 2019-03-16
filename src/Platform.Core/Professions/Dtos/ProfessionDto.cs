using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionDto
    {
        public int MinScore { get; set; }
        public ICollection<ProfessionTranslationsDto> Translations { get; set; }
    }
}
