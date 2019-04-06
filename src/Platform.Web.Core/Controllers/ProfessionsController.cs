using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Controllers
{
   // [Authorize(AuthenticationSchemes = "JwtBearer")]
    [DontWrapResult]
    [Expand(MaxDepth = 10)]
    public class ProfessionsController : AbpODataEntityController<Profession, long>, ITransientDependency
    {
        public ProfessionsController(IRepository<Profession,long> repository) : base(repository)
        {
        }

        //public override Task<IActionResult> Delete([FromODataUri] long key)
        //{
        //    return base.Delete(key);
        //}

        [EnableQuery(MaxExpansionDepth =4)]
        public override IQueryable<Profession> Get()
        {
            return base.Get();
        }
        [EnableQuery(MaxExpansionDepth = 4)]
        public override SingleResult<Profession> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }

        //public override Task<IActionResult> Patch([FromODataUri] long key, [FromBody] Delta<Profession> entity)
        //{
        //    return base.Patch(key, entity);
        //}

        //public override Task<IActionResult> Post([FromBody] Profession entity)
        //{
        //    return base.Post(entity);
        //}

        //public override Task<IActionResult> Put([FromODataUri] long key, [FromBody] Profession update)
        //{
        //    return base.Put(key, update);
        //}
    }
}
