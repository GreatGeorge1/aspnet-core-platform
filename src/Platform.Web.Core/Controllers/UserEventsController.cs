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
    public class UserEventsController : AbpODataEntityController<UserEvents, long>, ITransientDependency
    {
        public UserEventsController(IRepository<UserEvents, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<UserEvents> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<UserEvents> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }
    }
}
