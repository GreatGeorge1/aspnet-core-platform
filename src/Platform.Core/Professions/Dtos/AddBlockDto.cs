using System.Collections.Generic;
using Abp.AutoMapper;
using Platform.Professions.Dtos.Block;

namespace Platform.Professions.Dtos
{
    [AutoMapTo(typeof(Professions.Block))]
    public class AddBlockDto
    {
        public ICollection<CreateBlockTranslationsDto> Translations { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
    }
}
