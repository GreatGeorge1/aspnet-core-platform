using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class Block : FullAuditedEntity<long>, IPassivable, IMultiLingualEntity<BlockTranslations>
    {
        public bool IsActive { get; set; }
        public ICollection<BlockTranslations> Translations { get; set; }

        public int Index { get; set; }
        public int MinScore { get; set; }
        public Profession Profession { get; set; }

        public ICollection<StepBase> Steps { get; set; }

        //[NotMapped]
        //public int Duration
        //{ get { return Steps.Sum(s=>s.Duration);  }
        //    set { } }



    }

    [Audited]
    public class BlockTranslations : FullAuditedEntity<long>, IPassivable, IEntityTranslation<Block, long>, IMedia
    {
        public Block Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        public string VideoUrl { get; set; }
    }
}
