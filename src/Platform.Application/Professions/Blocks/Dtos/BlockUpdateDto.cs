using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Professions.Block))]
    public class BlockUpdateDto : EntityDto<long>
    {
        public bool IsActive { get; set; }
        public ICollection<BlockContentDto> Content { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
    }
}
