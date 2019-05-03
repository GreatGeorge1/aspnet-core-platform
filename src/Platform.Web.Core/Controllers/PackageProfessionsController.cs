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
    public class PackageProfessionsController : AbpODataEntityController<PackageProfession, long>, ITransientDependency
    {
        public PackageProfessionsController(IRepository<PackageProfession, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, PackageProfession update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(PackageProfession entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<PackageProfession> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }


}
