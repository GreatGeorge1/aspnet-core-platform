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
    public class EventTranslationsController : AbpODataEntityController<EventTranslations, long>, ITransientDependency
    {
        public EventTranslationsController(IRepository<EventTranslations, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, EventTranslations update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(EventTranslations entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<EventTranslations> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }


}
