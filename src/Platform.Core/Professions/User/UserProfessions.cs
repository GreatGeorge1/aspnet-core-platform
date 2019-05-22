using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions.User
{
    public class UserProfessions : AuditedEntity<long>, ISoftDelete
    {
        public Profession Profession { get; set; }
        public long ProfessionId { get; set; }
        public Authorization.Users.User User { get; set; }
        public long UserId { get; set; }


        public ICollection<UserTests> UserTests { get; set; }
        public ICollection<UserSeenSteps> UserSeenSteps { get; set; }
        public int Score { get; set; }
        /// <summary>
        /// False by Default
        /// </summary>
        public bool IsCompleted { get; set; }

        public void CalculateScore()
        {
            var score = 0;
            foreach(var item in this.UserTests)
            {
                foreach(var thing in item.UserTestAnswers)
                {
                    if (thing.Answer.IsCorrect)
                    {
                        score++;
                    }
                }
            }
            this.Score = score;
        }

        public void Init()
        {
            this.UserTests=new List<UserTests>();
            this.UserSeenSteps=new List<UserSeenSteps>();
            this.IsCompleted = false;
        }

        public bool IsDeleted { get; set; }
    }
}
