using System.Collections.Generic;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Professions.Block))]
    public class CreateBlockDto
    {
        public ICollection<CreateBlockTranslationsDto> Translations { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
        //public ICollection<StepBase> Steps { get; set; }
    }
}
