using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions.User;

namespace Platform.Professions
{
    [Audited]
    public class Answer : FullAuditedEntity<long>, IPassivable, IHasContent<Answer, AnswerContent, long>
    {
        public bool IsActive { get; set; }
        public AnswerContent Content { get; set; }
        public ICollection<UserTestAnswers> UserTestAnswers { get; set; }

        public bool IsCorrect { get; set; }
        public Step Test { get; set; }
    }
}