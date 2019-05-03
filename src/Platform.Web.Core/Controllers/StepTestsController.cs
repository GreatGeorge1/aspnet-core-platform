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
    public class StepTestsController : AbpODataEntityController<StepTest, long>, ITransientDependency
    {
        public StepTestsController(IRepository<StepTest, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, StepTest update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(StepTest entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<StepTest> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
