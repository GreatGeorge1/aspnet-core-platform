using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Packages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class PackageProfessionsController : AbpODataEntityController<PackageProfession, long>, ITransientDependency
    {
        public PackageProfessionsController(IRepository<PackageProfession, long> repository) : base(repository)
        {
        }
    }


}
