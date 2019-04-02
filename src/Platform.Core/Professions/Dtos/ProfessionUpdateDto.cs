using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionUpdateDto:ProfessionDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public new ICollection<UpdateProfessionTranslationDto> Translations { get; set; }
    }
}
