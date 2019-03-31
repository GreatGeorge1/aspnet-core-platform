using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class StepInfosController : AbpODataEntityController<StepInfo, long>, ITransientDependency
    {
        public StepInfosController(IRepository<StepInfo, long> repository) : base(repository)
        {
        }
    }
}
