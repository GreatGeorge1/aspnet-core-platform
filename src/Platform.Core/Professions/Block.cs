using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions
{
    [Audited]
    public class Block : AuditedEntity<long>, IDeletionAudited, IPassivable, IMultiLingualEntity<BlockTranslations>
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<BlockTranslations> Translations { get; set; }

        public int Index { get; set; }
        public int MinScore { get; set; }
        public Profession Profession { get; set; }

        public ICollection<StepBase> Steps { get; set; }
    }

    [Audited]
    public class BlockTranslations : AuditedEntity<long>, IEntityTranslation<Block, long>, IDeletionAudited, IPassivable, IMedia
    {
        public Block Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
    }
}
