using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Platform.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.Controllers
{
    public class EventsController : AbpODataEntityController<Event, long>, ITransientDependency
    {
        public EventsController(IRepository<Event, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<Event> Get()
        {
            return base.Get();
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<Event> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }
    }
}
