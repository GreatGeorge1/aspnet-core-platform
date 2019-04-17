using System;
using System.Linq;
using System.Reflection;
using Abp.AspNetCore;
using Abp.AspNetCore.OData.Configuration;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Platform.Authorization.Users;
using Platform.Configuration;
using Platform.Events;
using Platform.Identity;
using Platform.Packages;
using Platform.Professions;
using Platform.Web.Host.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Platform.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => 
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
                    options.Filters.Add<ResultFilter>();
                }
            );

            //services.AddDbContext<PlatformDbContext>(options =>
            // options.UseNpgsql(
            //    _appConfiguration.GetConnectionString("DefaultConnection")));

            IdentityRegistrar.Register(services);//.AddDefaultUI(UIFramework.Bootstrap4);

            
            
            AuthConfigurer.Configure(services, _appConfiguration);
            services.AddSignalR();
            services.AddOData();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        //.WithOrigins(
                        //    // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                        //    _appConfiguration["App:CorsOrigins"]
                        //        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        //        .Select(o => o.RemovePostFix("/"))
                        //        .ToArray()
                        //)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                       // .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Platform API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
               // var xml1 = "../Docs/Platform.Web.Core.xml";
               // var xml2 = "D:\\Projects\\Lab\\platform\\aspnet-core\\src\\DocsPlatform.Application.xml";
               // options.IncludeXmlComments(xml1);

            });


            // Workaround: https://github.com/OData/WebApi/issues/1177
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<PlatformWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = _appConfiguration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
          
            app.UseAbp(options => { options.UseAbpRequestLocalization = true; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            //app.UseJwtTokenMiddleware("IdentityBearer");
            //app.UseIdentityServer();

            app.UseAbpRequestLocalization();


            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });


            app.UseOData(builder =>
            {
                builder.EntitySet<Profession>("Professions").EntityType
                        .Filter() // Allow for the $filter Command
                        .Count() // Allow for the $count Command
                        .Expand(4) // Allow for the $expand Command
                        .OrderBy() // Allow for the $orderby Command
                        .Page() // Allow for the $top and $skip Commands
                        .Select();// Allow for the $select Command; 
                        
                        
                //builder.StructuralTypes.First(t => t.ClrType == typeof(Profession))
                //    .AddProperty(typeof(Profession).GetProperty("BlocksCount"));
                builder.EntitySet<ProfessionTranslations>("ProfessionTranslations").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand() // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<Block>("Blocks").EntityType
                      .Filter() // Allow for the $filter Command
                      .Count() // Allow for the $count Command
                      .Expand(4) // Allow for the $expand Command
                      .OrderBy() // Allow for the $orderby Command
                      .Page() // Allow for the $top and $skip Commands
                      .Select();// Allow for the $select Command; 
                     // .Property(b => b.StepsCount);

                builder.EntitySet<StepBase>("Steps").EntityType
                      .Filter() // Allow for the $filter Command
                      .Count() // Allow for the $count Command
                      .Expand(4) // Allow for the $expand Command
                      .OrderBy() // Allow for the $orderby Command
                      .Page() // Allow for the $top and $skip Commands
                      .Select();// Allow for the $select Command; 

                builder.EntitySet<StepInfo>("StepInfos").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<StepTest>("StepTests").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<Answer>("Answers").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<Package>("Packages").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<PackageTranslations>("PackageTranslations").EntityType
                      .Filter() // Allow for the $filter Command
                      .Count() // Allow for the $count Command
                      .Expand(4) // Allow for the $expand Command
                      .OrderBy() // Allow for the $orderby Command
                      .Page() // Allow for the $top and $skip Commands
                      .Select();// Allow for the $select Command; 

                builder.EntitySet<Event>("Events").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command;

                builder.EntitySet<EventTranslations>("EventTranslations").EntityType
                     .Filter() // Allow for the $filter Command
                     .Count() // Allow for the $count Command
                     .Expand(4) // Allow for the $expand Command
                     .OrderBy() // Allow for the $orderby Command
                     .Page() // Allow for the $top and $skip Commands
                     .Select();// Allow for the $select Command; 

                builder.EntitySet<EventProfession>("EventProfessions").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 
                builder.EntitySet<PackageProfession>("PackageProfessions").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

                builder.EntitySet<User>("Users").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 
                builder.EntitySet<UserEvents>("UserEvents").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 
                builder.EntitySet<Order>("Orders").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand(4) // Allow for the $expand Command
                       .OrderBy() // Allow for the $orderby Command
                       .Page() // Allow for the $top and $skip Commands
                       .Select();// Allow for the $select Command; 

            });

            // Return IQueryable from controllers
            app.UseUnitOfWork(options =>
            {
                options.Filter = httpContext =>
                {
                    return httpContext.Request.Path.Value.StartsWith("/odata");
                };
            });


            app.UseMvc(routes =>
            {
                routes.MapODataServiceRoute(app);
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Platform API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Platform.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
            app.UseReDoc(options => 
            {
                options.SpecUrl= $"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json";
                //options.IndexStream = () => Assembly.GetExecutingAssembly()
               // .GetManifestResourceStream("Platform.Web.Host.wwwroot.redoc.index.html");
                options.RoutePrefix = "redoc";
            });
        }
    }
}
