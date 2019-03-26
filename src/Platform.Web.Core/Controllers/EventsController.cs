using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class EventsController : AbpODataEntityController<Event, long>, ITransientDependency
    {
        public EventsController(IRepository<Event, long> repository) : base(repository)
        {
        }
    }
}
