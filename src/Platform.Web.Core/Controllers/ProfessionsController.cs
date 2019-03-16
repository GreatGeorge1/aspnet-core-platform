using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Controllers
{
    [DontWrapResult]
    public class ProfessionsController : AbpODataEntityController<Profession, long>, ITransientDependency
    {
        public ProfessionsController(IRepository<Profession,long> repository) : base(repository)
        {
        }

        public override Task<IActionResult> Delete([FromODataUri] long key)
        {
            return base.Delete(key);
        }

        public override IQueryable<Profession> Get()
        {
            return base.Get();
        }

        public override SingleResult<Profession> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }

        public override Task<IActionResult> Patch([FromODataUri] long key, [FromBody] Delta<Profession> entity)
        {
            return base.Patch(key, entity);
        }

        public override Task<IActionResult> Post([FromBody] Profession entity)
        {
            return base.Post(entity);
        }

        public override Task<IActionResult> Put([FromODataUri] long key, [FromBody] Profession update)
        {
            return base.Put(key, update);
        }
    }
}
