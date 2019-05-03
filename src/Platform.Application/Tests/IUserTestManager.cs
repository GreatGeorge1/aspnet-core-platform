using Abp.Domain.Services;
using Platform.Professions;
using Platform.Professions.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Tests
{
    public interface IUserTestManager : IDomainService
    {
        Task<int> SubmitTest(UserTestDto input);
        Task<ICollection<UserTests>> GetUserAnswers(long professionid, long blockid, long userid);
        Task<Dictionary<Block, ICollection<UserTests>>> GetUserAnswers(long professionid, long userid);
    }

    public class UserTestDto
    {
        public long UserId { get; set; }
        public long ProfessionId { get; set; }
        public long TestId { get; set; }
        public ICollection<long> AnswerIds { get; set; }
    }
}
