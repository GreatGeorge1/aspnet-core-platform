using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;

namespace Platform.Controllers
{
    public class StepInfosController : AbpODataEntityController<StepInfo, long>, ITransientDependency
    {
        public StepInfosController(IRepository<StepInfo, long> repository) : base(repository)
        {
        }
    }
}
