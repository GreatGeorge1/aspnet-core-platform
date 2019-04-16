using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Packages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Events
{
    [Audited]
    public class Event : AuditedEntity<long>, IDeletionAudited, IPassivable, IMultiLingualEntity<EventTranslations>
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<EventTranslations> Translations { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public ICollection<EventProfession> EventProfessions { get; set; }
        public ICollection<UserEvents> UserEvents { get; set; }
    }
}
