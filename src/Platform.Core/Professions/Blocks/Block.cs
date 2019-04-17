using System.Collections.Generic;
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
}
