using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions.User
{
    public class UserProfessions : AuditedEntity<long>
    {
        public Profession Profession { get; set; }
        public long ProfessionId { get; set; }
        public Authorization.Users.User User { get; set; }
        public long UserId { get; set; }
        public ICollection<UserTests> UserTests { get; set; }
        public int Score { get; set; }

        public void CalculateScore()
        {
            var score = 0;
            foreach(var item in this.UserTests)
            {
                foreach(var thing in item.Answers)
                {
                    if (thing.IsCorrect)
                    {
                        score++;
                    }
                }
            }
            this.Score = score;
        }
    }
    [Audited]
    public class UserTests : AuditedEntity<long>
    {
        public UserProfessions UserProfession { get; set; }
        public StepTest StepTest { get; set; }
        public ICollection<Answer> Answers {get;set;}
        public bool IsCorrect { get; set; }
    }
}
