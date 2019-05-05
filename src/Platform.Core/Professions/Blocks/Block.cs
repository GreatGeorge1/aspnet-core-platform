using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Platform.Professions.Blocks;

namespace Platform.Professions
{
    [Audited]
    public class Block : FullAuditedEntity<long>, IPassivable
    {
        public bool IsActive { get; set; }
        public ICollection<BlockContent> Content { get; set; }
        public int Index { get; set; }
        public int MinScore { get; set; }
       // public int Duration { get; private set; }
        public Profession Profession { get; set; }

        public ICollection<Step> Steps { get; set; }

        //private void CalculateDuration()
        //{
        //    int duration = 0;
            
        //    if (this.Steps.Any())
        //    {
        //        foreach (var item in this.Steps)
        //        {
        //            duration += item.Duration;
        //        }
        //    }
        //    this.Duration = duration;
        //}
    }
}
