using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Platform.Configuration;
using Abp.AspNetCore.OData;
using Abp.Authorization;
using Platform.Authentication.External;

namespace Platform.Web.Host.Startup
{
    [DependsOn(
       typeof(PlatformWebCoreModule), typeof(AbpAspNetCoreODataModule))]
    public class PlatformWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PlatformWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PlatformWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var externalAuthConfiguration = IocManager.Resolve<IExternalAuthConfiguration>();
            externalAuthConfiguration.Providers.Add(
                new ExternalLoginProviderInfo(
                   InstagramAuthProvider.Name,
                   "ecb21ea05f404e5ea836f8e39ca842ec",
                   "41c0a7977ff441e19d2d1ea877b164f0",
                   typeof(InstagramAuthProvider)
               )
           );
            externalAuthConfiguration.Providers.Add(
                 new ExternalLoginProviderInfo(
                    FacebookAuthProvider.Name,
                    "352709768684264",
                    "4576fa336138a84932b88ab6f41bf92e",
                    typeof(FacebookAuthProvider)
                )
            );

           
        }

    }
}
