﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Platform.Configuration;
using Abp.AspNetCore.OData;

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
    }
}
