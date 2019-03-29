using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos.Block
{
    [AutoMap(typeof(Professions.Block))]
    public class GetBlockAllDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
        public ICollection<GetBlockTranslationDto> Translations { get; set; }
      //  public ICollection<StepBase> Steps { get; set; }
    }
}
