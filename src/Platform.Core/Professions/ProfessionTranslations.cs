using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Platform.Professions
{
    [Audited]
    public class ProfessionTranslations : AuditedEntity<long>, IEntityTranslation<Profession, long>, IDeletionAudited, IPassivable
    {
        public Profession Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }
        public long? DeleterUserId { get ; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }
}