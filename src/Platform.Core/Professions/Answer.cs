using System;
using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class Answer : AuditedEntity<long>, IMultiLingualEntity<AnswerTranslation>, IDeletionAudited, IPassivable
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AnswerTranslation> Translations { get; set; }

        public bool IsCorrect { get; set; }
        public StepTest StepTest { get; set; }
    }
}