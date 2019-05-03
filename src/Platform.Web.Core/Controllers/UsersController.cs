using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Authorization;
using Platform.Authorization.Users;

namespace Platform.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtBearer")]
    public class UsersController : AbpODataEntityController<User, long>, ITransientDependency
    {
        public UsersController(IRepository<User, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<User> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<User> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }

        public override async Task<IActionResult> Post(User entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Put(long key, User update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<User> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }

}
