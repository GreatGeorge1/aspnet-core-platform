using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Platform.Events;
using Platform.Packages;
using Platform.Professions;
using Platform.Professions.Blocks;
using Platform.Professions.User;

namespace Platform.EntityFrameworkCore.Seed.Tenants
{
    public class ProfessionBuilder
    {
        private readonly PlatformDbContext _context;
        private readonly int _tenantId;

        public ProfessionBuilder(PlatformDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateFullProfession();
        }

        private void CreateFullProfession()
        {
            // Profession with content
            var profession = _context.Professions
                .Include(p=>p.Content)
                .Include(p=>p.Author)
                .Include(p=>p.Blocks)
                .Include(p=>p.Event)
                .Include(p=>p.Package)
                .Include(p=>p.UserProfessions)
                .IgnoreQueryFilters().FirstOrDefault();
            if (profession == null)
            {
                profession = Profession.CreateTestProfession(true);
                var content = new ProfessionContent
                {
                    Title = "Test_Profession",
                    Description = "Test_Profession",
                    Language = "ru-RU",
                    Core = profession,
                    IsActive = false
                };
                profession.Content=content;
            }

            //Block with content
            if (!profession.Blocks.Any())
            {
                var block = Block.CreateTestBlock(true);
                //block.Content.Add();
                var content = new BlockContent
                {
                    Title = "Test_Block", Description = "Test_Block", Language = "ru-RU", Core = block
                };
                block.Content=content;
                block.Profession = profession;
                
                //Steps
                block.Steps=new List<Step>();
                //info
                var info = new Step()
                {
                    Block = block,
                    IsActive = false,
                    Index = 0,
                    Duration = 5,
                    Type = StepType.Info,
                    UserSeenSteps = new List<UserSeenSteps>(),
                    //Content=new List<StepContent>()
                };
                info.Content=new StepContent()
                {
                    Core=info,
                    Title="StepInfo",
                    Description = "desc",
                    Language = "ru-RU"
                };
                block.Steps.Add(info);
                //test
                var test = new Step()
                {
                    Block = block,
                    IsActive = false,
                    Index = 0,
                    Duration = 5,
                    Type = StepType.Test,
                    UserSeenSteps = new List<UserSeenSteps>(),
                    //Content=new List<StepContent>(),
                    Answers = new List<Answer>()
                };
                info.Content=new StepContent()
                {
                    Core=test,
                    Title="StepTest",
                    Description = "desc",
                    Language = "ru-RU"
                };

                var ans1 = new Answer()
                {
                   // Content = new List<AnswerContent>(),
                    UserTestAnswers = new List<UserTestAnswers>(),
                    IsCorrect = true,
                    IsActive = false,
                    Test = test
                };
                ans1.Content=new AnswerContent()
                {
                    Core=ans1,
                    Title = "Da",
                    Description = "sdsdsdssd",
                    Language = "ru-RU"
                };
                var ans2 = new Answer()
                {
                   // Content = new List<AnswerContent>(),
                    UserTestAnswers = new List<UserTestAnswers>(),
                    IsCorrect = false,
                    IsActive = false,
                    Test = test
                };
                ans1.Content=new AnswerContent()
                {
                    Core=ans1,
                    Title = "Net",
                    Description = "sdsdsdssd",
                    Language = "ru-RU"
                };
                test.Answers.Add(ans1);
                test.Answers.Add(ans2);
                block.Steps.Add(test);
                //open 
                var open = new Step()
                {
                    Block = block,
                    IsActive = false,
                    Index = 0,
                    Duration = 5,
                    Type = StepType.Open,
                    UserSeenSteps = new List<UserSeenSteps>(),
                   // Content=new List<StepContent>(),
                    Answers = new List<Answer>()
                };
                info.Content=new StepContent()
                {
                    Core=test,
                    Title="StepOpen",
                    Description = "desc",
                    Language = "ru-RU"
                };
                block.Steps.Add(open);
                
                profession.Blocks.Add(block);
            }

            //Authors
            if (profession.Author == null)
            {
                var author = new Author
                {
                    Name = "Author",
                    Base64Image = "image",
                    Professions = new List<Profession>()
                };
                profession.Author = author;
                author.Professions.Add(profession);
            }

            {
                var author = new Author
                {
                    Name = "Author2",
                    Base64Image = "image",
                    Professions = new List<Profession>()
                };
                _context.Authors.Add(author);
            }
            
            //Package
            if (profession.Package == null)
            {
                //profession.Packages=new List<Package>();
                var package = new Package()
                {
                    Profession = profession,
                    Price = 150,
                 //   IsActive = false,
                    OrderPackages = new List<OrderPackages>()
                };
                profession.Package=package;
            }
            
            //Event
            if (profession.Event == null)
            {
               // profession.Events=new List<Event>();
                var event1 = new Event()
                {
                    IsActive = false,
                    DateStart = new DateTime(2019, 5, 10),
                    DateEnd = new DateTime(2019, 12, 31),
                    Profession = profession
                };
                profession.Event=event1;
            }
            
            _context.Professions.Add(profession);
            var prof2= Profession.CreateTestProfession(true);
            prof2.Id = 100;
            _context.Professions.Add(prof2);   
            var prof3= Profession.CreateTestProfession(true);
            prof3.Id = 101;
            _context.Professions.Add(prof3);  
            var prof4= Profession.CreateTestProfession(true);
            prof4.Id = 102;
            _context.Professions.Add(prof4);  
            //userprofessions
            var up1 = new UserProfessions();
            up1.Init();
            up1.ProfessionId = 101;
            up1.UserId = 2;
            _context.UserProfessions.Add(up1);
            var up2 = new UserProfessions();
            up2.Init();
            up2.ProfessionId = 102;
            up2.UserId = 2;
            _context.UserProfessions.Add(up2);
            
            _context.SaveChanges();
        }
    }
}