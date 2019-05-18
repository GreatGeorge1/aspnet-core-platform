using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Platform.Professions.User
{
    public class UserTestAnswers : Entity<long>
    {
        public long UserTestId { get; set; }
        [Required]
        public UserTests UserTest { get; set; }
        public Answer Answer { get; set; }
        public long AnswerId { get; set; }
        public string Text { get; set; }
        [Required]
        [EnumDataType(typeof(StepType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AnswerType Type { get; set; }
    }
    
    public enum AnswerType
    {
        Open,
        Test
    }
}
