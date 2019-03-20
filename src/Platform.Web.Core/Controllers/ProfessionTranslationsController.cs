using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Controllers
{
    public class ProfessionTranslationsController : AbpODataEntityController<ProfessionTranslations, long>, ITransientDependency
    {
        public ProfessionTranslationsController(IRepository<ProfessionTranslations, long> repository) : base(repository)
        {
        }
    }
}
