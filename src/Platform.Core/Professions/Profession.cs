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
    public class Profession : FullAuditedEntity<long>, IPassivable, IHasContent<Profession, ProfessionContent, long>
    {
        public bool IsActive { get; set; }
        //public ICollection<ProfessionContent> Content { get; set; }
        public ProfessionContent Content { get; set; }
        public ICollection<Block> Blocks { get; set; }
       // public ICollection<Package> Packages { get; set; }
        public Package Package { get; set; }
        // public ICollection<Event> Events { get; set; }
        public Event Event { get; set; }
        public ICollection<UserProfessions> UserProfessions { get; set; }
        public Author Author { get; set; }
        public void SetAuthor(Author author)
        {
            this.Author = author;
        }

        public static Profession CreateTestProfession(bool isActive)
        {
            var profession = new Profession();
           // profession.Content=new List<ProfessionContent>();
            profession.Blocks=new List<Block>();
           // profession.Events=new List<Event>();
           // profession.Packages=new List<Package>();
            profession.UserProfessions=new List<UserProfessions>();
            profession.IsActive = isActive;
            return profession;
        }

        public void Update(Profession newprof)
        {
            if (newprof.Package != null)
            {
                newprof.Package.Profession = this;
                this.Package = newprof.Package;
            }

            if (newprof.Event != null)
            {
                newprof.Event.Profession = this;
                this.Event = newprof.Event;
            }

            if (newprof.IsActive != this.IsActive)
            {
                
            }
        }
    }
}
