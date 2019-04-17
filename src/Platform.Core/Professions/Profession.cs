using System.Collections.Generic;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Events;
using Platform.Packages;

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

        //[NotMapped]
        //public int Duration
        //{
        //    get { return Blocks.Sum(s => s.Duration); }
        //    set { }
        //}

        //[NotMapped]
        //public int MinScore
        //{
        //    get { return Blocks.Sum(s => s.MinScore); }
        //    set { }
        //}

        //[NotMapped]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public int BlocksCount => Blocks.Count;

    }
}
