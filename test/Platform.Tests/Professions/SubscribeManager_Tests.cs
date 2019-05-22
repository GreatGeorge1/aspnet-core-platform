using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Platform.Professions;
using Platform.Professions.User;
using Platform.Subscribes;
using Shouldly;
using Xunit;

namespace Platform.Tests.Professions
{
    public class SubscribeManager_Tests:PlatformTestBase
    {
        private readonly ISubscribeManager _subscribeManager;

        public SubscribeManager_Tests()
        {
            _subscribeManager = Resolve<ISubscribeManager>();
        }

        [Fact]
        public async Task SubscribeToProfession_2_1()
        {
            await _subscribeManager.SubscribeToProfession(2, 1);
            await UsingDbContextAsync(async context =>
            {
                var userprofession = await context.UserProfessions.Include(up=>up.Profession)
                    .Include(up=>up.User).LastOrDefaultAsync();
                Assert.NotNull(userprofession);
                userprofession.User.Name.ShouldBe("admin");
                userprofession.Profession.Id.ShouldBe(1);
            });
        }
        
        [Fact]
        public async Task SubscribeToProfession_Multiple()
        {
            await _subscribeManager.SubscribeToProfession(2, 1);
            await _subscribeManager.SubscribeToProfession(2, 100);
            await UsingDbContextAsync(async context =>
            {
                var userprofessions = context.UserProfessions.Include(up => up.Profession)
                    .Include(up => up.User).ToList();
                Assert.NotNull(userprofessions);
               // userprofessions.Count.ShouldBe(3);
                var up1=userprofessions.Find(up => up.Id == 1);
                up1.ShouldNotBeNull();
                var up2= userprofessions.Find(up => up.Id == 100);
                up2.ShouldBeNull();
            
            });
        }

        [Fact]
        public async Task UnsubscribeToProfession_2_101()
        {
            await _subscribeManager.UnsubscribeToProfession(2, 101);
            await UsingDbContextAsync(async context =>
            {
                var userprofessions = context.UserProfessions.Include(up => up.Profession)
                    .Include(up => up.User).ToList();
                var up1=userprofessions.Find(up => up.Id == 101);
                up1.ShouldBeNull();
            });
        }
        
        [Fact]
        public async Task SubscribeToProfession_0_1()
        {
            Func<Task> res = () => _subscribeManager.SubscribeToProfession(0, 1);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }
        [Fact]
        public async Task SubscribeToProfession_1_0()
        {
            Func<Task> res = () => _subscribeManager.SubscribeToProfession(0, 1);
            var ex = await Record.ExceptionAsync(res);
            Assert.NotNull(ex);
            Assert.IsType<EntityNotFoundException>(ex);
        }

        [Fact]
        public async Task GetSubscrpitions_2()
        {
            var subs = await _subscribeManager.GetSubscriptions(2);
            Assert.IsType<List<UserProfessions>>(subs);
            subs.Count.ShouldBe(2);
            subs.SingleOrDefault(up => up.ProfessionId==101).ShouldNotBeNull();
            subs.SingleOrDefault(up => up.ProfessionId==102).ShouldNotBeNull();
        }

        [Fact]
        public async Task IsSubscribed_2_101()
        {
            var res = await _subscribeManager.UserIsSubscribed(2,101);
            res.ShouldBe(true);
        }
        [Fact]
        public async Task IsSubscribed_2_1()
        {
            var res = await _subscribeManager.UserIsSubscribed(2,1);
            res.ShouldBe(false);
        }
    }
}