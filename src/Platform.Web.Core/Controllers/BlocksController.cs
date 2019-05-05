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
    [Expand(MaxDepth = 10)]
    public class BlocksController : AbpODataEntityController<Block, long>, ITransientDependency
    {
        public BlocksController(IRepository<Block, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, Block update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Block entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Block> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
