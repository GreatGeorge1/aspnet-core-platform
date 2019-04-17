using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public abstract class StepBase : FullAuditedEntity<long>, IPassivable, IMultiLingualEntity<StepTranslations>
    {
        public bool IsActive { get; set; }
        public ICollection<StepTranslations> Translations { get; set; }
        public int Duration { get; set; }
        public Block Block { get; set; }
        public int Index { get; set; }
    }

    public class StepInfo : StepBase
    { }

    public class StepTest : StepBase
    {
        public ICollection<Answer> Answers { get; set; }
    }
}
