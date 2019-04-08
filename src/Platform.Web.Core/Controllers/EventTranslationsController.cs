using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class EventTranslationsController : AbpODataEntityController<EventTranslations, long>, ITransientDependency
    {
        public EventTranslationsController(IRepository<EventTranslations, long> repository) : base(repository)
        {
        }
    }


}
