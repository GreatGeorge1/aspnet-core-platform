using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class Answer : FullAuditedEntity<long>, IPassivable, IMultiLingualEntity<AnswerTranslation>
    {
        public bool IsActive { get; set; }
        public ICollection<AnswerTranslation> Translations { get; set; }

        public bool IsCorrect { get; set; }
        public StepTest StepTest { get; set; }
    }
}