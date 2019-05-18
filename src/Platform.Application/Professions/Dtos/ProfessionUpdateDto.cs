using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Events.Dtos;
using Platform.Packages.Dtos;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class ProfessionUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public PackageDto Package { get; set; }
        public EventDto Event { get; set; }
        //public int MinScore { get; set; }
    }
}
