using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
       // public int BlocksCount { get; private set; }

        //private void CalculateBlocksCount()
        //{
        //    int count = 0;
        //    if (this.Blocks.Any())
        //    {
        //        count = this.Blocks.Count; 
        //    }
        //    this.BlocksCount= count;
        //}

        //public int Duration { get; private set; }
       // public virtual int MinScore { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProfessionContent> Content { get; set; }
        public ICollection<Block> Blocks { get; set; }
        public ICollection<Package> Packages { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<UserProfessions> UserProfessions { get; set; }
        public Author Author { get; set; }
        public void SetAuthor(Author author)
        {
            this.Author = author;
        }

        //public int CalculateMinScore()
        //{
        //    int score = 0;
        //    try
        //    {
        //        if (this.Blocks.Any())
        //        {
        //            foreach (var item in this.Blocks)
        //            {
        //                score += item.MinScore;
        //            }
        //        }
        //    }catch(Exception e)
        //    {

        //    }
        //    return score;
        //}

        //private int CalculateDuration()
        //{
        //    int duration = 0;
        //    try
        //    {
        //        if (this.Blocks.Any())
        //        {
        //            foreach (var item in this.Blocks)
        //            {
        //                duration += item.Duration;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return duration;
        //}
    }
}
