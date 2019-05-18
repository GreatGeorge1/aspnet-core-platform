using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Platform.Professions;
using Platform.Professions.Dtos;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Platform.Tests.Professions
{
    public class StepAppService_Tests:PlatformTestBase
    {
        private readonly IStepAppService _stepAppService;

        public StepAppService_Tests()
        {
            _stepAppService = Resolve<IStepAppService>();
        }
        
        [Fact]
        public async Task Create_Should_Throw()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _stepAppService.Create(new StepCreateDto()));
        }
        
         [Fact]
        public async Task UpdateContent_Id_0()
        {
            var dto = new StepContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=0
            };
            Func<Task> res = () =>  _stepAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }
        
        [Fact]
        public async Task UpdateContent_Id_NotExisting()
        {
            var dto = new StepContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=3456
            };
            Func<Task> res = () =>  _stepAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }

        [Fact]
        public async Task UpdateContent_IsActive_True()
        {
            var dto = new StepContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=1
            };
            _=await _stepAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var block = await context.Steps.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
                block.ShouldNotBeNull();
                block.Content.ShouldNotBeNull();
                var content = block.Content;
                content.ShouldNotBeNull();
                content.Description.ShouldBe("update");
                content.Title.ShouldBe("update");
                content.IsActive.ShouldBe(true);
            });
        }
        
        [Fact]
        public async Task UpdateContent_IsActive_False()
        {
            var dto = new StepContentUpdateDto()
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = false,
                Id=1
            };
            _=await _stepAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var block = await context.Steps.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
                block.ShouldNotBeNull();
                block.Content.ShouldNotBeNull();
                var content = block.Content;
                content.ShouldNotBeNull();
                content.Description.ShouldBe("update");
                content.Title.ShouldBe("update");
                content.IsActive.ShouldBe(false);
            });
        }

        [Fact]
        public async Task CreateAnswer_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _stepAppService.CreateAnswer(new AnswerCreateDto(),0));
        }
        [Fact]
        public async Task CreateAnswer_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _stepAppService.CreateAnswer(new AnswerCreateDto(),4545));
        }
        [Fact]
        public async Task CreateAnswer_StepType_Info()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _stepAppService.CreateAnswer(new AnswerCreateDto(),1));
        }
        
        [Fact]
        public async Task CreateAnswer_Correct_True()
        {
            var dto = new AnswerCreateDto()
            {
                IsActive = false,
                IsCorrect = true,
             //   Content = new List<AnswerContentDto>()
            };
            dto.Content=new AnswerContentDto()
            {
                Title = "True",
                Description = "desc",
                Language = "ru-RU",
                IsActive = false
            };
            await _stepAppService.CreateAnswer(dto, 2);
            await UsingDbContextAsync(async context =>
            {
                var step = await context.Steps.Include(s => s.Answers).ThenInclude(a => a.Content)
                    .FirstOrDefaultAsync(s => s.Id == 2);
                step.ShouldNotBeNull();
                step.Answers.ShouldNotBeNull();
                step.Answers.Any().ShouldNotBeNull();
                Answer answer = context.Answers.Include(a => a.Content).Include(a => a.Test).LastOrDefault();
                answer.ShouldNotBeNull();
                answer.IsActive.ShouldBe(false);
                answer.IsCorrect.ShouldBe(true);
                answer.Test.Type.ShouldBe(StepType.Test);
                answer.Content.IsActive.ShouldBe(false);
                answer.Content.Title.ShouldBe("True");
            });
        }
        
        [Fact]
        public async Task CreateAnswer_Correct_False()
        {
            var dto = new AnswerCreateDto()
            {
                IsActive = true,
                IsCorrect = false,
             //   Content = new List<AnswerContentDto>()
            };
            dto.Content=new AnswerContentDto()
            {
                Title = "False",
                Description = "desc",
                Language = "ru-RU",
                IsActive = true
            };
            await _stepAppService.CreateAnswer(dto, 2);
            await UsingDbContextAsync(async context =>
            {
                var step = await context.Steps.Include(s => s.Answers).ThenInclude(a => a.Content)
                    .FirstOrDefaultAsync(s => s.Id == 2);
                step.ShouldNotBeNull();
                step.Answers.ShouldNotBeNull();
                step.Answers.Any().ShouldNotBeNull();
                Answer answer = context.Answers.Include(a => a.Content).Include(a => a.Test).LastOrDefault();
                answer.ShouldNotBeNull();
                answer.IsActive.ShouldBe(true);
                answer.IsCorrect.ShouldBe(false);
                answer.Test.Type.ShouldBe(StepType.Test);
                answer.Content.IsActive.ShouldBe(true);
                answer.Content.Title.ShouldBe("False");
            });
        }

        [Fact]
        public async Task DeleteAnswer_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _stepAppService.DeleteAnswer(new AnswerDeleteDto()
                {
                    AnswerId = 0
                }));
        }
        
        [Fact]
        public async Task DeleteAnswer_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _stepAppService.DeleteAnswer(new AnswerDeleteDto()
                {
                    AnswerId = 4545
                }));
        }
        
        [Fact]
        public async Task DeleteAnswer_Id_1()
        {
            await _stepAppService.DeleteAnswer(new AnswerDeleteDto()
                {
                    AnswerId = 1
                });
            await UsingDbContextAsync(async context =>
            {
                var answer = await context.Answers.FirstOrDefaultAsync(a => a.Id == 1);
                answer.ShouldNotBeNull();
                answer.IsDeleted.ShouldBe(true);
            });
        }
        
        [Fact]
        public async Task GetById_Id_1()
        {
            var step = await _stepAppService.Get(new StepDto()
            {
                Id = 1
            });
            step.ShouldNotBeNull();
        }
        [Fact]
        public async Task GetById_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async()=>await _stepAppService.Get(new StepDto()
            {
                Id = 0
            }));
        }
        [Fact]
        public async Task GetById_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async()=>await _stepAppService.Get(new StepDto()
            {
                Id = 4545
            }));
        }
    }
}