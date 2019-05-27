using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Platform.Professions.User;
using Shouldly;
using Xunit;

namespace Platform.Tests.Professions
{
    public class UserTestManager_Tests:PlatformTestBase
    {
        private readonly IUserTestManager _userTestManager;
        
        public UserTestManager_Tests()
        {
            _userTestManager=Resolve<IUserTestManager>();
        }
        
        [Fact]
        public async Task SubmitTest()
        {
            var testinput = new UserTestDto()
            {
                UserId=2,
                ProfessionId = 1,
                TestId = 2,
                AnswerIds = new long[] {1},
                Type= AnswerType.Test 
            };
            await _userTestManager.SubmitTest(testinput);
            await UsingDbContextAsync(async context =>
            {
                var res = await context.UserTests.Include(up=>up.UserTestAnswers)
                    .ThenInclude(up=>up.Answer)
                    .Include(up=>up.UserProfession)
                    .Include(up=>up.Test).LastOrDefaultAsync();
                Assert.NotNull(res);
                res.IsCorrect.ShouldBe(true);
                res.UserProfession.ProfessionId.ShouldBe(1);
                res.Test.Id.ShouldBe(2);
                res.UserTestAnswers.First().Answer.Id.ShouldBe(1);
            });
        }
        
//        [Fact]
//        public async Task SubscribeToProfession_Multiple()
//        {
//            await _subscribeManager.SubscribeToProfession(2, 1);
//            await _subscribeManager.SubscribeToProfession(2, 100);
//            await UsingDbContextAsync(async context =>
//            {
//                var userprofessions = context.UserProfessions.Include(up => up.Profession)
//                    .Include(up => up.User).ToList();
//                Assert.NotNull(userprofessions);
//                // userprofessions.Count.ShouldBe(3);
//                var up1=userprofessions.Find(up => up.Id == 1);
//                up1.ShouldNotBeNull();
//                var up2= userprofessions.Find(up => up.Id == 100);
//                up2.ShouldBeNull();
//            
//            });
//        }
    }
}