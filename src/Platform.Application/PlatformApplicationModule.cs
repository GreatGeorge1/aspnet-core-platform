﻿using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Platform.Authorization;

namespace Platform
{
    [DependsOn(
        typeof(PlatformCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PlatformApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PlatformAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PlatformApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
