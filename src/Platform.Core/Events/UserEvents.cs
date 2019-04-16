﻿using Abp.Domain.Entities.Auditing;
using Platform.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events
{
    public class UserEvents:AuditedEntity<long>
    {
        public int Score { get; set; }
        public bool IsCompleted { get; set; }
        public long EventId { get; set; }
        public Event Event { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
