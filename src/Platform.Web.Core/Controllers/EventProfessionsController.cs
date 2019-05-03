using System;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Platform.Events;

namespace Platform.Controllers
{

    public class EventProfessionsController : AbpODataEntityController<EventProfession, long>, ITransientDependency
    {
        public EventProfessionsController(IRepository<EventProfession, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, EventProfession update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(EventProfession entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<EventProfession> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
