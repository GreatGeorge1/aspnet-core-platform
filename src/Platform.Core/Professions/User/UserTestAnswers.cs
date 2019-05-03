using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Professions.User
{
    public class UserTestAnswers : Entity<long>
    {
        public long UserTestId { get; set; }
        public UserTests UserTest { get; set; }
        public Answer Answer { get; set; }
        public long AnswerId { get; set; }
    }
}
