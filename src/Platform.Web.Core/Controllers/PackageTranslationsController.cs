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
    public class PackageTranslationsController : AbpODataEntityController<PackageTranslations, long>, ITransientDependency
    {
        public PackageTranslationsController(IRepository<PackageTranslations, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, PackageTranslations update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(PackageTranslations entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<PackageTranslations> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
