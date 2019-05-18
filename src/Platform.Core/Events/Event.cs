using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions;

namespace Platform.Events
{
    [Audited]
    public class Event : AuditedEntity<long>, IDeletionAudited, IPassivable
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        [Required]
        public Profession Profession { get; set; }
        public long ProfessionId { get; set; }
    }
}
