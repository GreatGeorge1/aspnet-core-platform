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
    public class Block : FullAuditedEntity<long>, IPassivable, IHasContent<Block, BlockContent, long>
    {
        public bool IsActive { get; set; }
        public BlockContent Content { get; set; }
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

        public static Block CreateTestBlock(bool isActive)
        {
            var block=new Block();
           // block.Content=new List<BlockContent>();
            block.IsActive = isActive;
            block.Steps=new List<Step>();
            block.Index = 0;
            block.MinScore = 0;
            return block;
        }
    }
}
