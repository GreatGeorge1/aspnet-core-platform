using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Platform.Professions;
using Platform.Professions.Blocks;
using Platform.Professions.Dtos;
using Shouldly;
using Xunit;

namespace Platform.Tests.Professions
{
    public class ProfessionAppService_Tests : PlatformTestBase
    {
        private readonly IProfessionAppService _professionAppService;
        public ProfessionAppService_Tests()
        {
            _professionAppService = Resolve<IProfessionAppService>();
        }

        [Fact]
        public async Task Create_Test()
        {
            var content = new ProfessionContentDto()
            {
                Title = "Test",
                Description = "Description",
                Base64Image = "image",
                VideoUrl = "https://stackoverflow.com/questions/45017295/assert-an-exception-using-xunit",
                Id = 0,
                IsActive = false,
                Language = "ru-RU"
            };
            var prof = new ProfessionCreateDto()
            {
                IsActive = false,
              //  Content = new List<ProfessionContentDto>(),
            };
            prof.Content=content;
            
            await _professionAppService.Create(prof);
            
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "Test");
                profession.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task UpdateContent_ShouldThrowUserFriendlyException()
        {
            var dto = new ProfessionContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=0
            };
            Func<Task> res = () =>  _professionAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }
        
        [Fact]
        public async Task UpdateContent_Id_NotExisting()
        {
            var dto = new ProfessionContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=3456
            };
            Func<Task> res = () =>  _professionAppService.UpdateContent(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }

        [Fact]
        public async Task UpdateContent_IsActive_True()
        {
            var dto = new ProfessionContentUpdateDto
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = true,
                Id=1
            };
            _=await _professionAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
                profession.ShouldNotBeNull();
                profession.Content.ShouldNotBeNull();
                var content = profession.Content;
                content.ShouldNotBeNull();
                content.Description.ShouldBe("update");
                content.Title.ShouldBe("update");
                content.IsActive.ShouldBe(true);
            });
        }
        
        [Fact]
        public async Task UpdateContent_IsActive_False()
        {
            var dto = new ProfessionContentUpdateDto()
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = false,
                Id=1
            };
            _=await _professionAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
                profession.ShouldNotBeNull();
                profession.Content.ShouldNotBeNull();
                var content = profession.Content;
                content.ShouldNotBeNull();
                content.Description.ShouldBe("update");
                content.Title.ShouldBe("update");
                content.IsActive.ShouldBe(false);
            });
        }
        
        [Fact]
        public async Task UpdateContent_IsActive_Null()
        {
            var dto = new ProfessionContentUpdateDto()
            {
                Base64Image = "",
                Description = "update",
                VideoUrl = null,
                Title = "update",
                IsActive = null,
                Id=1
            };
            _=await _professionAppService.UpdateContent(dto);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Content).FirstOrDefaultAsync(p => p.Content.Title == "update");
                profession.ShouldNotBeNull();
                profession.Content.ShouldNotBeNull();
                var content = profession.Content;
                content.ShouldNotBeNull();
                content.Description.ShouldBe("update");
                content.Title.ShouldBe("update");
                content.IsActive.ShouldBe(false);
            });
        }
        
        [Fact]
        public async Task DeleteBlock_Id_1()
        {
            var dto = new BlockDeleteDto()
            {
               BlockId = 1
            };
            await _professionAppService.DeleteBlock(dto);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Blocks).FirstOrDefaultAsync(p => p.Id==1);
                profession.ShouldNotBeNull();
                profession.Blocks.ShouldNotBeNull();
                var block = profession.Blocks.FirstOrDefault(b=>b.Id==dto.BlockId);
                block.ShouldNotBeNull();
                block.IsDeleted.ShouldBe(true);
            });
        }
        
        [Fact]
        public async Task DeleteBlock_Id_NotExisting()
        {
            var dto = new BlockDeleteDto()
            {
                BlockId = 25
            };
            Func<Task> res = () =>  _professionAppService.DeleteBlock(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }
        
        [Fact]
        public async Task DeleteBlock_Id_0()
        {
            var dto = new BlockDeleteDto()
            {
                BlockId = 0
            };
            Func<Task> res = () =>  _professionAppService.DeleteBlock(dto);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }

        [Fact]
        public async Task CreateBlock_Id_0()
        {
            var dto = new BlockCreateDto()
            {
                MinScore = 10,
                IsActive = true,
                Index=1,
             //   Content = new List<BlockContentDto>()
            };
            dto.Content= new BlockContentDto()
            {
                Title = "TestBlock1",
                Description = "desc",
                Language = "ru-RU"
            };
            Func<Task> res = () =>  _professionAppService.CreateBlock(dto, 0);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }
        
        [Fact]
        public async Task CreateBlock_Id_NotExisting()
        {
            var dto = new BlockCreateDto()
            {
                MinScore = 10,
                IsActive = true,
                Index=1,
              //  Content = new List<BlockContentDto>()
            };
            dto.Content= new BlockContentDto()
            {
                Title = "TestBlock1",
                Description = "desc",
                Language = "ru-RU"
            };
            Func<Task> res = () =>  _professionAppService.CreateBlock(dto, 456564);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }
        
        [Fact]
        public async Task CreateBlock_Id_1()
        {
            var dto = new BlockCreateDto()
            {
                MinScore = 10,
                IsActive = true,
                Index=1,
               // Content = new List<BlockContentDto>()
            };
            dto.Content= new BlockContentDto()
            {
                Title = "TestBlock1",
                Description = "desc",
                Language = "ru-RU"
            };
            await _professionAppService.CreateBlock(dto, 1);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Blocks).ThenInclude(b=>b.Content).FirstOrDefaultAsync(p => p.Id == 1);
                profession.ShouldNotBeNull();
                profession.Blocks.ShouldNotBeNull();
                var block = profession.Blocks.FirstOrDefault(b => b.Content.Title == "TestBlock1");
                block.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task SetAuthor_Id_1_Author_2()
        {
            await _professionAppService.SetAuthor(id: 1, authorid: 2);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Author).FirstOrDefaultAsync(p => p.Author.Id == 2);
                profession.ShouldNotBeNull();
            });
        }
        [Fact]
        public async Task SetAuthor_Id_1_Author_1()
        {
            await _professionAppService.SetAuthor(id: 1, authorid: 1);
            await UsingDbContextAsync(async context =>
            {
                var profession = await context.Professions.Include(p=>p.Author).FirstOrDefaultAsync(p => p.Author.Id == 1);
                profession.ShouldNotBeNull();
            });
        }
        
        [Fact]
        public async Task SetAuthor_Id_0_Author_1_ShouldThrow()
        {
            Func<Task> res = () =>   _professionAppService.SetAuthor(id: 0, authorid: 1);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }
        
        [Fact]
        public async Task SetAuthor_Id_1_Author_0_ShouldThrow()
        {
            Func<Task> res = () =>   _professionAppService.SetAuthor(id: 1, authorid: 0);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<UserFriendlyException>(ex);
        }

        [Fact]
        public async Task GetById_Id_1()
        {
            var profession = await _professionAppService.Get(new ProfessionDto()
            {
                Id = 1
            });
            profession.ShouldNotBeNull();
        }
        [Fact]
        public async Task GetById_Id_0()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async()=>await _professionAppService.Get(new ProfessionDto()
            {
                Id = 0
            }));
        }
        [Fact]
        public async Task GetById_Id_NotExisting()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async()=>await _professionAppService.Get(new ProfessionDto()
            {
                Id = 4545
            }));
        }

//        [Fact]
//        public async Task Update_Id_0()
//        {
//            await _professionAppService.Update(new ProfessionUpdateDto()
//            {
//                Id = 0
//            });
//        }
    }
}