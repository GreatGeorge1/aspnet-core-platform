using System.Linq;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Platform.Authorization.Users;

namespace Platform.Controllers
{
    public class UsersController : AbpODataEntityController<User, long>, ITransientDependency
    {
        public UsersController(IRepository<User, long> repository) : base(repository)
        {
        }

        [EnableQuery(MaxExpansionDepth = 0)]
        public override IQueryable<User> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<User> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }
    }

}
