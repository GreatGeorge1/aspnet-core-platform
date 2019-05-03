using System;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Platform.Packages;

namespace Platform.Controllers
{
    public class PackagesController : AbpODataEntityController<Package, long>, ITransientDependency
    {
        public PackagesController(IRepository<Package, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, Package update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Package entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Package> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
