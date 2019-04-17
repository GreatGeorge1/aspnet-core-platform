using System.Collections.Generic;
using Abp.AutoMapper;

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
