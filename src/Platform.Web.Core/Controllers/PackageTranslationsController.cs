using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Packages;

namespace Platform.Controllers
{
    public class PackageTranslationsController : AbpODataEntityController<PackageTranslations, long>, ITransientDependency
    {
        public PackageTranslationsController(IRepository<PackageTranslations, long> repository) : base(repository)
        {
        }
    }
}
