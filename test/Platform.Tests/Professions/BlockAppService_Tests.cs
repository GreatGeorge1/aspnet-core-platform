using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Platform.Professions;
using Platform.Professions.Dtos;
using Shouldly;
using Xunit;

namespace Platform.Tests.Professions
{
    public class BlockAppService_Tests:PlatformTestBase
    {
        private readonly IBlockAppService _blockAppService;
        public BlockAppService_Tests()
        {
            _blockAppService = Resolve<IBlockAppService>();
        }

        [Fact]
        public async Task BlockCreate_ShouldThrow()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async()=>await _blockAppService.Create(new BlockCreateDto()));
        }

        [Fact]
        public async Task CreateStep_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _blockAppService.CreateStep(new StepCreateDto(), 0));
        }
        
        [Fact]
        public async Task CreateStep_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _blockAppService.CreateStep(new StepCreateDto(), 25));
        }

        [Fact]
        public async Task CreateStep_TypeInfo_Id_1()
        {
            var dto = new StepCreateDto()
            {
                Duration = 5,
                Index = 1,
                IsActive = false,
               // Content = new List<StepContentDto>(),
                Type = StepType.Info
            };
            dto.Content=new StepContentDto()
            {
                Title = "stepcreate_test",
                Description = "sdsdsdsdsds",
                Language = "ru-RU",
                IsActive = false,
            };
            var res=await _blockAppService.CreateStep(dto,1);
            await UsingDbContextAsync(async context =>
                {
                    var block= await context.Blocks.Include(b => b.Steps).ThenInclude(s => s.Content)
                        .FirstOrDefaultAsync(b => b.Id == 1);
                    block.ShouldNotBeNull();
                    block.Steps.ShouldNotBeNull();
                    block.Steps.Any().ShouldBe(true);
                    var step = block.Steps.LastOrDefault();
                    step.ShouldNotBeNull();
                    step.Duration.ShouldBe(5);
                    step.Index.ShouldBe(1);
                    step.IsActive.ShouldBe(false);
                    step.Type.ShouldBe(StepType.Info);
                    step.Content.IsActive.ShouldBe(false);
                });
        }
        
        [Fact]
        public async Task CreateStep_TypeOpen_Id_1()
        {
            var dto = new StepCreateDto()
            {
                Duration = 5,
                Index = 1,
                IsActive = false,
              //  Content = new List<StepContentDto>(),
                Type = StepType.Open
            };
            dto.Content=new StepContentDto()
            {
                Title = "stepcreate_test",
                Description = "sdsdsdsdsds",
                Language = "ru-RU",
                IsActive = false,
            };
            var res=await _blockAppService.CreateStep(dto,1);
            await UsingDbContextAsync(async context =>
            {
                var block= await context.Blocks.Include(b => b.Steps).ThenInclude(s => s.Content)
                    .FirstOrDefaultAsync(b => b.Id == 1);
                block.ShouldNotBeNull();
                block.Steps.ShouldNotBeNull();
                block.Steps.Any().ShouldBe(true);
                var step = block.Steps.LastOrDefault();
                step.ShouldNotBeNull();
                step.Duration.ShouldBe(5);
                step.Index.ShouldBe(1);
                step.IsActive.ShouldBe(false);
                step.Type.ShouldBe(StepType.Open);
                step.Content.IsActive.ShouldBe(false);
            });
        }
        
        [Fact]
        public async Task CreateStep_TypeTest_Id_1()
        {
            var dto = new StepCreateDto()
            {
                Duration = 5,
                Index = 1,
                IsActive = true,
            //    Content = new List<StepContentDto>(),
                Type = StepType.Test,
            };
            dto.Content=new StepContentDto()
            {
                Title = "stepcreate_test2",
                Description = "sdsdsdsdsds",
                Language = "ru-RU",
                IsActive = false
            };
            var res=await _blockAppService.CreateStep(dto,1);
            await UsingDbContextAsync(async context =>
            {
                var block= await context.Blocks.Include(b => b.Steps).ThenInclude(s => s.Content)
                    .FirstOrDefaultAsync(b => b.Id == 1);
                block.ShouldNotBeNull();
                block.Steps.ShouldNotBeNull();
                block.Steps.Any().ShouldBe(true);
                Step step = context.Steps.Include(s => s.Content).LastOrDefault();
                step.ShouldNotBeNull();
                step.Duration.ShouldBe(5);
                step.Index.ShouldBe(1);
                step.IsActive.ShouldBe(true);
                step.Type.ShouldBe(StepType.Test);
                step.Content.IsActive.ShouldBe(false);
            });
        }
        

        [Fact]
        public async Task DeleteStep_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _blockAppService.DeleteStep(new StepDeleteDto()
                {
                    StepId = 0
                }));
        }
        
        [Fact]
        public async Task DeleteStep_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _blockAppService.DeleteStep(new StepDeleteDto()
                {
                    StepId = 4554
                }));
        }
        
        [Fact]
        public async Task DeleteStep_Id_1()
        {
            await _blockAppService.DeleteStep(new StepDeleteDto()
            {
                StepId = 1
            });
            await UsingDbContextAsync(async context =>
            {
                var block= await context.Steps
                    .FirstOrDefaultAsync(b => b.Id == 1);
                block.ShouldNotBeNull();
                block.IsDeleted.ShouldBe(true);
            });
        }
        
        [Fact]
        public async Task UpdateContent_Id_0()
        {
            var dto = new BlockContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=0
            };
            Func<Task> res = () =>  _blockAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }
        
        [Fact]
        public async Task UpdateContent_Id_NotExisting()
        {
            var dto = new BlockContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=3456
            };
            Func<Task> res = () =>  _blockAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }

        [Fact]
        public async Task UpdateContent_IsActive_True()
        {
            var dto = new BlockContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=1
            };
            _=await _blockAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var block = await context.Blocks.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
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
            var dto = new BlockContentUpdateDto()
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = false,
                Id=1
            };
            _=await _blockAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var block = await context.Blocks.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
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
        public async Task UpdateContent_IsActive_Null()
        {
            var dto = new BlockContentUpdateDto()
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = null,
                Id=1
            };
            _=await _blockAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var block = await context.Blocks.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
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
        public async Task GetById_Id_1()
        {
            var block = await _blockAppService.Get(new BlockDto()
            {
                Id = 1
            });
            block.ShouldNotBeNull();
        }
        [Fact]
        public async Task GetById_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async()=>await _blockAppService.Get(new BlockDto()
            {
                Id = 0
            }));
        }
        [Fact]
        public async Task GetById_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async()=>await _blockAppService.Get(new BlockDto()
            {
                Id = 4545
            }));
        }
        
    }
}