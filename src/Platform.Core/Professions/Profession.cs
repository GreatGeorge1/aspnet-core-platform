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
    public class Profession : FullAuditedEntity<long>, IPassivable
    {
        //public virtual int? MinScore { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProfessionContent> Content { get; set; }
        public ICollection<Block> Blocks { get; set; }
        public ICollection<Package> Packages { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<UserProfessions> UserProfessions { get; set; }

    }
}
