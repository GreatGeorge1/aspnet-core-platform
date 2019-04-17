using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;

namespace Platform.Controllers
{
    public class AnswersController : AbpODataEntityController<Answer, long>, ITransientDependency
    {
        public AnswersController(IRepository<Answer, long> repository) : base(repository)
        {
        }
    }


}
