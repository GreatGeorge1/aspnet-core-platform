using System;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;

namespace Platform.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtBearer")]
    [Expand(MaxDepth = 4)]
    public class AnswersController : AbpODataEntityController<Answer, long>, ITransientDependency
    {
        public AnswersController(IRepository<Answer, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, Answer update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Answer entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Answer> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }


}
