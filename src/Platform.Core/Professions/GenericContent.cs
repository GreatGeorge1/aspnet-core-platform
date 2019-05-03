using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions
{
    public abstract class GenericContent<TCore, TKey> : FullAuditedEntity<TKey>, IPassivable, IMedia
        where TCore : IEntity<TKey>
    {
        public TCore Core { get; set; }
        public long CoreId { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        [Url]
        public string VideoUrl { get; set; }
    }
}
