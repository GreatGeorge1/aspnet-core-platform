using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Platform.Professions.Block))]
    public class CreateBlockDto
    {
        public ICollection<CreateBlockTranslationsDto> Translations { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
        //public ICollection<StepBase> Steps { get; set; }
    }
}
