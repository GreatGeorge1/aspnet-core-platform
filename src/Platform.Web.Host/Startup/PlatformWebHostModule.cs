using Abp.AspNetCore.OData;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Platform.Authentication.External;
using Platform.Configuration;

namespace Platform.Web.Host.Startup
{
    [DependsOn(
       typeof(PlatformWebCoreModule), typeof(AbpAspNetCoreODataModule),typeof(AbpAspNetCoreSignalRModule))]
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
                   "2fb449df1bd84999898a1a87a81f939c",
                   "7413c43d4590436db88b8708eda073fb",
                   typeof(InstagramAuthProvider)
               )
           );
            externalAuthConfiguration.Providers.Add(
                 new ExternalLoginProviderInfo(
                    FacebookAuthProvider.Name,
                    "452281665522672",
                    "0b92d9516e76f82580526cd57167ac65",
                    typeof(FacebookAuthProvider)
                )
            );

           
        }

    }
}
