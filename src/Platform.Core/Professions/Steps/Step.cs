using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Platform.Professions.User;

namespace Platform.Professions
{
    [Audited]
    public class Step : FullAuditedEntity<long>, IPassivable, IHasContent<Step, StepContent, long>
    {
        public bool IsActive { get; set; }
        public StepContent Content { get; set; }
        public int Duration { get; set; }
        public Block Block { get; set; }
        public int Index { get; set; }

        [Required]
        [EnumDataType(typeof(StepType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public StepType Type {get;set;}  
        public ICollection<Answer> Answers { get; set; }
        public ICollection<UserSeenSteps> UserSeenSteps { get; set; }
        public ICollection<UserTests> UserTests { get; set; }
    }

    public enum StepType
    {
        Info,
        Test,
        Open
    }
   
}
