using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Professions;

namespace Platform.Controllers
{
   // [Authorize(AuthenticationSchemes = "JwtBearer")]
    public class AuthorsController:AbpODataEntityController<Author, long>, ITransientDependency
    {
        public AuthorsController(IRepository<Author, long> repository) : base(repository)
        {
        }
        
        public override async Task<IActionResult> Put(long key, Author update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Author entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Author> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
    
}