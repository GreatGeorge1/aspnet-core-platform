using Abp.Application.Services;
using Abp.Authorization;
using Platform.Authorization;
using Platform.Tests.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.UI;
using JetBrains.Annotations;
using Platform.Professions.User;

namespace Platform.Tests
{
    [AbpAuthorize]
    public class UserTestsAppService : ApplicationService, IUserTestAppService
    {
        private readonly IUserTestManager userTestManager;
        [NotNull] private readonly IBackgroundJobManager _backgroundJobManager;

        public UserTestsAppService(IUserTestManager userTestManager,
            IBackgroundJobManager backgroundJobManager)
        {
            this.userTestManager = userTestManager ?? throw new ArgumentNullException(nameof(userTestManager));
            this._backgroundJobManager = backgroundJobManager ??
                                         throw new ArgumentNullException(nameof(backgroundJobManager));
        }

        public async Task<SubmitResponseDto> Submit(UserTestDto input)
        {
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != input.UserId)
                {
                    throw new AbpAuthorizationException("You are not authorized to submit this test!");
                }
            }
            if (input.Type == AnswerType.Open)
            {
                await userTestManager.SubmitOpen(input);
                return new SubmitResponseDto { CorrectCount = 0, AnswersCount = 0 };
            }
            var res = await userTestManager.SubmitTest(input);
            
            return new SubmitResponseDto { CorrectCount = res, AnswersCount = input.AnswerIds.Count };
        }
        
        

        public async Task<UserAnswersDto> GetUserAnswersForProfession(long userid, long professionid)
        {
            if (userid == 0)
            {
                throw new UserFriendlyException("userid в url не может быть 0 или null");
            }

            if (professionid == 0)
            {
                throw new UserFriendlyException("professionid в url не может быть 0 или null");
            }
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != userid)
                {
                    throw new AbpAuthorizationException("You are not authorized to get answers for this user!");
                }
            }
            var res = await userTestManager.GetUserAnswers(professionid: professionid, userid: userid);
            var temp = new UserAnswersDto { ProfessionId = professionid, BlockAnswers = new List<BlockAnswers>() };
            foreach (var item in res)
            {
                var usertests = new List<UserTestResponseDto>();
                foreach (var thing in item.Value)
                {
                    usertests.Add(ObjectMapper.Map<UserTestResponseDto>(thing));
                }
                temp.BlockAnswers.Add(new BlockAnswers { BlockId = item.Key.Id, UserTests = usertests });
                
            }
            return temp;
        }
        public async Task<UserAnswersDto> GetUserAnswersForBlock(long userid, long professionid, long blockid)
        {
            if (userid == 0)
            {
                throw new UserFriendlyException("userid в url не может быть 0 или null");
            }
            if (professionid == 0)
            {
                throw new UserFriendlyException("professionid в url не может быть 0 или null");
            }
            if (blockid == 0)
            {
                throw new UserFriendlyException("blockid в url не может быть 0 или null");
            }
            
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != userid)
                {
                    throw new AbpAuthorizationException("You are not authorized to get answers for this user!");
                }
            }
            var res = await userTestManager.GetUserAnswers(professionid: professionid, userid: userid, blockid: blockid);
            var temp = new UserAnswersDto { ProfessionId = professionid, BlockAnswers = new List<BlockAnswers>() };

            var usertests = new List<UserTestResponseDto>();
            foreach (var thing in res)
            {
                usertests.Add(ObjectMapper.Map<UserTestResponseDto>(thing));
            }
            temp.BlockAnswers.Add(new BlockAnswers { BlockId = blockid, UserTests = usertests });

            return temp;
        }
    }
}
