using System.Collections.Generic;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos.Step
{
    [AutoMap(typeof(StepInfo))]
    public class GetInfoStepAllDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public int Index { get; set; }
        public ICollection<UpdateStepTranslationDto> Translations { get; set; }
        public int Duration { get; set; }
        //public Block Block { get; set; }
    }
}
