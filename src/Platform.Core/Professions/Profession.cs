using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Events;
using Platform.Packages;
using Platform.Professions.User;

namespace Platform.Professions
{
    [Audited]
   // [Table("AppProfessions")]
    public class Profession : FullAuditedEntity<long>, IPassivable, IMultiLingualEntity<ProfessionTranslations>
    {
        //public virtual int? MinScore { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ProfessionTranslations> Translations { get; set; }

        public ICollection<Block> Blocks { get; set; }

        public ICollection<PackageProfession> PackageProfessions { get; set; }
        public ICollection<EventProfession> EventProfessions { get; set; }

        public ICollection<UserProfessions> UserProfessions { get; set; }

    }
}
