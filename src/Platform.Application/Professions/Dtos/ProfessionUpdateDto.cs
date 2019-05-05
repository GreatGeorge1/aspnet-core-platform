using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        //public int MinScore { get; set; }
    }
}
