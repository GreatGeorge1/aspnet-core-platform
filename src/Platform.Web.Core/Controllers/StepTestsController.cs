using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;

namespace Platform.Controllers
{
    public class StepTestsController : AbpODataEntityController<StepTest, long>, ITransientDependency
    {
        public StepTestsController(IRepository<StepTest, long> repository) : base(repository)
        {
        }
    }
}
