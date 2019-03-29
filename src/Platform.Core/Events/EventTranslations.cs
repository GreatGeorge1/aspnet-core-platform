using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions;

namespace Platform.Events
{
    [Audited]
    public class EventTranslations : AuditedEntity<long>, IDeletionAudited, IPassivable, IEntityTranslation<Event, long>, IMedia
    {
        public Event Core { get; set; }
        public long CoreId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
       
        public string Language { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}