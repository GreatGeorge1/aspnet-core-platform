using System;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class StepTranslations : AuditedEntity<long>, IEntityTranslation<StepBase, long>, IDeletionAudited, IPassivable, IMedia
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public StepBase Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }
}