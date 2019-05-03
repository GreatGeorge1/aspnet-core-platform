using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Authorization;
using Platform.Authorization.Users;
using Platform.Professions;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;

namespace Platform.Controllers
{
    //[AbpAuthorize]
    //[Authorize(AuthenticationSchemes = "JwtBearer")]
    [DontWrapResult]
    [Expand(MaxDepth = 10)]
    public class ProfessionsController : AbpODataEntityController<Profession, long>, ITransientDependency
    {
       // private readonly IRepository<Profession, long> repository;
        private readonly IRepository<User, long> userRepository;
        private readonly IPermissionChecker permissionChecker;

        public ProfessionsController(IRepository<Profession, long> repository, IPermissionChecker permissionChecker, IRepository<User, long> userRepository) : base(repository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
        }


        [EnableQuery(MaxExpansionDepth =0)]
        public override IQueryable<Profession> Get()
        {
           
            //if (!permissionChecker.IsGranted(PermissionNames.Pages_Users))
            //{
            //    if ()
            //    {
            //        throw new AbpAuthorizationException("You are not authorized to submit this test!");
            //    }
            //}
            return base.Get();
            //return repository.GetAllIncluding(p => p.EventProfessions).AsQueryable();
        }
        [EnableQuery(MaxExpansionDepth = 0)]
        public override SingleResult<Profession> Get([FromODataUri] long key)
        {
            return base.Get(key);
        }

        public override async Task<IActionResult> Put(long key, Profession update)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Post(Profession entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Patch(long key, Delta<Profession> entity)
        {
            return new NotFoundResult();
        }

        public override async Task<IActionResult> Delete(long key)
        {
            return new NotFoundResult();
        }
    }
}
