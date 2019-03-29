using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Platform.Professions
{
    [Audited]
    public class StepTranslations : FullAuditedEntity<long>, IPassivable, IEntityTranslation<StepBase, long>, IMedia
    {
        public bool IsActive { get; set; }
        public StepBase Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}