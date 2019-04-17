using System.Linq;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Platform.Packages;

namespace Platform.Controllers
{
    public class OrdersController : AbpODataEntityController<Order, long>, ITransientDependency
    {
        public OrdersController(IRepository<Order, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<Order> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<Order> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }
    }
}
