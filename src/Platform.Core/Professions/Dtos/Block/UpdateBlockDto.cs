using System.Collections.Generic;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Professions.Block))]
    public class UpdateBlockDto:CreateBlockDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public new ICollection<UpdateBlockTranslationDto> Translations { get; set; }
    }
}
