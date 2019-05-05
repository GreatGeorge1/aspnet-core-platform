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

        public virtual void Update<TContent>(TContent newcontent) where TContent : GenericContent<TCore, TKey>
        {
            if (newcontent.Title != null)
            {
                this.Title = newcontent.Title;
            }

            if (newcontent.Description != null)
            {
                this.Description = newcontent.Description;
            }

            if (newcontent.Base64Image != null)
            {
                this.Base64Image = newcontent.Base64Image;
            }

            if (newcontent.VideoUrl != null)
            {
                this.VideoUrl = newcontent.VideoUrl;
            }

            if (newcontent.Language != null)
            {
                this.Language = newcontent.Language;
            }
        }
    }
}
