using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions.User;

namespace Platform.Professions
{
    [Audited]
    public class Answer : FullAuditedEntity<long>, IPassivable
    {
        public bool IsActive { get; set; }
        public ICollection<AnswerContent> Content { get; set; }
        public ICollection<UserTestAnswers> UserTestAnswers { get; set; }

        public bool IsCorrect { get; set; }
        public Step Test { get; set; }
    }
}