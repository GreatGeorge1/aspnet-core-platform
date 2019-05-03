using System;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;

namespace Platform.Controllers
{
    public class ProfessionTranslationsController : AbpODataEntityController<ProfessionTranslations, long>, ITransientDependency
    {
        public ProfessionTranslationsController(IRepository<ProfessionTranslations, long> repository) : base(repository)
        {
        }

        public override async Task<IActionResult> Put(long key, ProfessionTranslations update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(ProfessionTranslations entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<ProfessionTranslations> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
