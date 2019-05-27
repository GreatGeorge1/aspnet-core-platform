using Abp.Domain.Services;
using Platform.Professions;
using Platform.Professions.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Platform.Tests
{
    public interface IUserTestManager : IDomainService
    {
        Task<int> SubmitTest(UserTestDto input);
        Task<ICollection<UserTests>> GetUserAnswers(long professionid, long blockid, long userid);
        Task<Dictionary<Block, ICollection<UserTests>>> GetUserAnswers(long professionid, long userid);
        Task<int> SubmitOpen(UserTestDto input);
    }

    public class UserTestDto
    {
        public long UserId { get; set; }
        public long ProfessionId { get; set; }
        public long TestId { get; set; }
        public ICollection<long> AnswerIds { get; set; }
        public string Text { get; set; }
        //[Required]
        //[EnumDataType(typeof(StepType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AnswerType Type { get; set; }
    }
    
}
