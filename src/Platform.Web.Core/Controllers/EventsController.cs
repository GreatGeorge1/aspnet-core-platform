using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Platform.Events;

namespace Platform.Controllers
{
    [Expand(MaxDepth = 10)]
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

        public override async Task<IActionResult> Put(long key, Event update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Event entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Event> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
