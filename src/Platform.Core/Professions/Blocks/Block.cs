using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions.Blocks;

namespace Platform.Professions
{
    [Audited]
    public class Block : FullAuditedEntity<long>, IPassivable
    {
        public bool IsActive { get; set; }
        public ICollection<BlockContent> Content { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
        public Profession Profession { get; set; }

        public ICollection<Step> Steps { get; set; }
    }
}
