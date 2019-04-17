using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Events;

namespace Platform.Controllers
{
    public class EventProfessionsController : AbpODataEntityController<EventProfession, long>, ITransientDependency
    {
        public EventProfessionsController(IRepository<EventProfession, long> repository) : base(repository)
        {
        }
    }
}
