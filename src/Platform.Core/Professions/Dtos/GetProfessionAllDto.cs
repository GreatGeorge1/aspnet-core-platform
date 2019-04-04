using Abp.AutoMapper;
using Platform.Professions.Dtos.Block;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(Profession))]
    public class GetProfessionAllDto
    {
        public long Id;
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        long? CreatorUserId { get; set; }
        public int MinScore { get; set; }
        public ICollection<ProfessionTranslationsDto> Translations { get; set; }
        public ICollection<GetBlockAllDto> Blocks { get; set; }
    }
}
