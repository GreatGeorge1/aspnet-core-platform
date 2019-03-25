using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions
{
    [Audited]
    public abstract class StepBase : AuditedEntity<long>, IDeletionAudited, IPassivable, IMultiLingualEntity<StepTranslations>
    {
        public bool IsActive { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<StepTranslations> Translations { get; set; }

        public DateTime Duration { get; set; }
        public Block Block { get; set; }
    }

    public class StepInfo : StepBase
    { }

    public class StepTest : StepBase
    {
        public ICollection<Answer> Answers { get; set; }
    }
}
