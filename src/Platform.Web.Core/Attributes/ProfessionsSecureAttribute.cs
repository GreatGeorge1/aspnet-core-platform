//using Abp.Authorization;
//using Abp.Domain.Repositories;
//using Microsoft.AspNet.OData;
//using Microsoft.AspNet.OData.Query;
//using Microsoft.AspNetCore.Http;
//using Platform.Authorization;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Platform.Attributes

//    internal class ProfessionsSecureAttribute : EnableQueryAttribute
//    {
//        private readonly IPermissionChecker permissionChecker;
//        private readonly IRepository<Users, long> 

//        public ProfessionsSecureAttribute(IPermissionChecker permissionChecker)
//        {
//            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
//        }

//        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
//        {
//            if (queryOptions.SelectExpand != null
//            && queryOptions.SelectExpand.RawExpand != null
//            && queryOptions.SelectExpand.RawExpand.Contains("Orders"))
//            {
//                //Check here if user is allowed to view orders.
//                if (!permissionChecker.IsGranted(PermissionNames.Pages_Users))
//                {
//                    if ()
//                    {
//                        throw new AbpAuthorizationException("You are not authorized to submit this test!");
//                    }
//                }
//                throw new InvalidOperationException();
//            }

//            base.ValidateQuery(request, queryOptions);
//        }
//    }
//}
