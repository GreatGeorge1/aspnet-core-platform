using System;
using System.Net;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;

namespace Platform.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtBearer")]
    public class StepsController : AbpODataEntityController<StepBase, long>, ITransientDependency
    {
        public StepsController(IRepository<StepBase, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, StepBase update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(StepBase entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<StepBase> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
