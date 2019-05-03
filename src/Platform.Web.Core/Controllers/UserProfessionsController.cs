using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtBearer")]
    public class UserProfessionsController : AbpODataEntityController<UserProfessions, long>, ITransientDependency
    {
        public UserProfessionsController(IRepository<UserProfessions, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<UserProfessions> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<UserProfessions> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }

        public override async Task<IActionResult> Put(long key, UserProfessions update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<UserProfessions> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(UserProfessions entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }

}
