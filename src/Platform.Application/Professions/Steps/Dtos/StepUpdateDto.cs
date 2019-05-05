using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Step))]
    public class StepUpdateDto : EntityDto<long>
    {
        //[Required]
        //public StepType Type { get; set; }
        public bool IsActive { get; set; }
        //public ICollection<StepContentDto> Content { get; set; }
        public int Duration { get; set; }
        public int Index { get; set; }
    }
}
