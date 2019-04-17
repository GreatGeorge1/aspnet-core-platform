using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Packages;

namespace Platform.Controllers
{
    public class PackageProfessionsController : AbpODataEntityController<PackageProfession, long>, ITransientDependency
    {
        public PackageProfessionsController(IRepository<PackageProfession, long> repository) : base(repository)
        {
        }
    }


}
