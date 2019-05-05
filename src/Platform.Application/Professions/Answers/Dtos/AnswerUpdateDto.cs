using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Platform.Professions.Answer))]
    public class AnswerUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        //public ICollection<AnswerContentDto> Content { get; set; }
        public bool IsCorrect { get; set; }
    }

}
