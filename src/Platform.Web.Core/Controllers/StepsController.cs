﻿using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class StepsController : AbpODataEntityController<StepBase, long>, ITransientDependency
    {
        public StepsController(IRepository<StepBase, long> repository) : base(repository)
        {
        }
    }
}
