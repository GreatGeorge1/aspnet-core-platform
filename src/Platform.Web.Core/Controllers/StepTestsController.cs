using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class StepTestsController : AbpODataEntityController<StepTest, long>, ITransientDependency
    {
        public StepTestsController(IRepository<StepTest, long> repository) : base(repository)
        {
        }
    }
}
