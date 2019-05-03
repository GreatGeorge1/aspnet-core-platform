﻿using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization.Users;
using Platform.Packages;
using Platform.Professions;
using Platform.Professions.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Tests
{
    public class UserTestManager : DomainService, IUserTestManager
    {
        private readonly UserManager userManager;
        private readonly IRepository<Profession, long> professionRepository;
        private readonly IRepository<Answer, long> answerRepository;
        private readonly IRepository<UserProfessions, long> userProfessionsRepository;
        private readonly IRepository<StepTest, long> stepTestRepository;

        public UserTestManager(UserManager userManager, IRepository<Profession, long> professionRepository, IRepository<Answer, long> answerRepository, IRepository<UserProfessions, long> userProfessionsRepository, IRepository<StepTest, long> stepTestRepository)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.professionRepository = professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
            this.userProfessionsRepository = userProfessionsRepository ?? throw new ArgumentNullException(nameof(userProfessionsRepository));
            this.stepTestRepository = stepTestRepository ?? throw new ArgumentNullException(nameof(stepTestRepository));
        }

        public async Task<int> SubmitTest(UserTestDto input)
        {
            var user = await userManager.GetUserByIdAsync(input.UserId) ?? throw new ArgumentNullException(nameof(User));
            var profession = await GetProfession(input.ProfessionId) ?? throw new ArgumentNullException(nameof(Profession));
            UserProfessions userprofession;
            try
            {
                userprofession = await GetUserProfession(userid: user.Id, professionid: profession.Id) ?? throw new ArgumentNullException(nameof(UserProfessions));
            }catch(ArgumentNullException e)
            {
                userprofession = await CreateAndGetUserProfession(prof: profession, user: user);
            }

            var steptest = await SearchTestInProfession(profession, input.TestId) ?? throw new ArgumentNullException(nameof(StepTest));
            var answerlist = await SearchAnswersForTestByIds(steptest.Id, input.AnswerIds) ?? throw new ArgumentNullException(nameof(ICollection<Answer>));

            int scorecount = 0;
            foreach (var item in answerlist)
            {
                if (item.IsCorrect)
                {
                    scorecount++;
                }
            }
            UserTests usertest;
            try
            {
               usertest = await FindUserTestByTestId(testid: steptest.Id, userprofession: userprofession) ?? throw new ArgumentNullException(nameof(UserTests));    
            }
            catch(ArgumentNullException ex)
            {
                usertest = new UserTests { StepTest = steptest, UserProfession = userprofession, Answers = new List<Answer>() };
            }


            if (!usertest.Answers.Any())
            {
                usertest.Answers = answerlist.ToList();
                userprofession.UserTests.Add(usertest);
            }
            else
            {
                usertest.Answers = answerlist.ToList();
            }
            userprofession.CalculateScore();
            await userProfessionsRepository.InsertOrUpdateAsync(userprofession);
            return scorecount;
        }

        private async Task<ICollection<Answer>> SearchAnswersForTestByIds(long stepTestId,ICollection<long> AnswerIds)
        {
            var answerlist = new List<Answer>();
            foreach (var item in AnswerIds)
            {
                var temp = await answerRepository.GetAllIncluding(a=>a.StepTest).FirstOrDefaultAsync(a => a.Id == item && a.StepTest.Id == stepTestId);
                if (temp != null)
                {
                    answerlist.Add(temp);
                }
            }
            return answerlist;
        }

        private async Task<UserTests> FindUserTestByTestId(long testid, UserProfessions userprofession)
        {
            if (!userprofession.UserTests.Any())
            {
                return null;
            }
            return await userprofession.UserTests.AsQueryable().FirstOrDefaultAsync(ut => ut.StepTest.Id == testid);
        }

        private async Task<StepTest> SearchTestInProfession(Profession prof, long testId)
        {
            StepTest steptest = null;
            foreach (var item in prof.Blocks)
            {
                steptest = await stepTestRepository.GetAllIncluding(s => s.Answers, s => s.Block).FirstOrDefaultAsync(st => st.Block.Id == item.Id && st.Id == testId);
                if (steptest != null)
                {
                    break;
                }
            }
            return steptest;
        }     

        private async Task<Profession> GetProfession(long profId)
        {
            return await professionRepository.GetAll()
                .Include(p => p.Blocks)
                .ThenInclude(b=>b.Steps)
                .FirstOrDefaultAsync(p => p.Id == profId);
        }

        private async Task<UserProfessions> GetUserProfession(long userid, long professionid)
        {
            var temp = await userProfessionsRepository.FirstOrDefaultAsync(up => up.ProfessionId == professionid && up.UserId == userid);
            return await GetUserProfession(temp.Id);
            //return await userProfessionsRepository.GetAll()
            //    .Include(up => up.UserTests).ThenInclude(ut => ut.Answers)
            //    .Include(up => up.UserTests).ThenInclude(ut => ut.StepTest)
            //    .Include(up=>up.User)
            //    .Include(up=>up.Profession)
            //    .FirstOrDefaultAsync(up => up.Profession.Id==professionid && up.User.Id==userid);
        }

        private async Task<UserProfessions> GetUserProfession(long userprofessionid)
        {
            return await userProfessionsRepository.GetAll()
                .Include(up => up.UserTests).ThenInclude(ut => ut.Answers)
                .Include(up => up.UserTests).ThenInclude(ut => ut.StepTest)
                .FirstOrDefaultAsync(up => up.Id == userprofessionid);
        }

        private async Task<UserProfessions> CreateAndGetUserProfession(Profession prof, User user)
        {
            var newid = await userProfessionsRepository.InsertAndGetIdAsync(new UserProfessions { Profession = prof, User = user, UserTests = new List<UserTests>() });
            var userprofession = await GetUserProfession(newid);
            return userprofession;
        }

        public async Task<ICollection<UserTests>> GetUserAnswers(long professionid, long blockid, long userid)
        {
            var user = await userManager.GetUserByIdAsync(userid) ?? throw new ArgumentException("User does not exist");
            var profession = await GetProfession(professionid) ?? throw new ArgumentException("Profession does not exist");
            var userprofession = await GetUserProfession(userid: user.Id, professionid: profession.Id);
            if (userprofession == null)
            {
                userprofession = await CreateAndGetUserProfession(prof: profession, user: user);
            }
            var block=await profession.Blocks.AsQueryable().FirstOrDefaultAsync(b=>b.Id==blockid) ?? throw new ArgumentException($"block {blockid} not exist in profession {professionid}");
            return await SearchUserTestsByBlock(userprofession: userprofession, block: block);
        }

        private async Task<ICollection<UserTests>> SearchUserTestsByBlock(UserProfessions userprofession, Block block)
        {
            var list = new List<UserTests>();
            foreach (var item in userprofession.UserTests)
            {
                foreach (var thing in block.Steps)
                {
                    if (item.StepTest.Id == thing.Id)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public async Task<Dictionary<Block,ICollection<UserTests>>> GetUserAnswers(long professionid, long userid)
        {
            var user = await userManager.GetUserByIdAsync(userid) ?? throw new ArgumentException("User does not exist");
            var profession = await GetProfession(professionid) ?? throw new ArgumentException("Profession does not exist");
            var userprofession = await GetUserProfession(userid: user.Id, professionid: profession.Id);
            if (userprofession == null)
            {
                userprofession = await CreateAndGetUserProfession(prof: profession, user: user);
            }
            var dictionary = new Dictionary<Block, ICollection<UserTests>>();
            foreach(var item in profession.Blocks)
            {
                var temp = await SearchUserTestsByBlock(userprofession: userprofession, block: item);
                dictionary.Add(item, temp);
            }
            return dictionary;
        }
    }
}
