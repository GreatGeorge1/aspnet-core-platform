using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Platform.Professions.User
{
    [Audited]
    public class UserTests : AuditedEntity<long>
    {
        public UserProfessions UserProfession { get; set; }
        public Step Test { get; set; }// Step.Type==Test
        public ICollection<UserTestAnswers> UserTestAnswers {get;set;}
        public bool IsCorrect { get; set; }
    }
}
