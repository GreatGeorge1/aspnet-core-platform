using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Packages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class PackagesController : AbpODataEntityController<Package, long>, ITransientDependency
    {
        public PackagesController(IRepository<Package, long> repository) : base(repository)
        {
        }
    }
}
