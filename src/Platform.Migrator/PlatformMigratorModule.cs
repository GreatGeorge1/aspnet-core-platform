using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using Platform.Configuration;
using Platform.EntityFrameworkCore;
using Platform.Migrator.DependencyInjection;

namespace Platform.Migrator
{
    [DependsOn(typeof(PlatformEntityFrameworkModule))]
    public class PlatformMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public PlatformMigratorModule(PlatformEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(PlatformMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                PlatformConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PlatformMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
