﻿using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class BlocksController : AbpODataEntityController<Block, long>, ITransientDependency
    {
        public BlocksController(IRepository<Block, long> repository) : base(repository)
        {
        }
    }
}
