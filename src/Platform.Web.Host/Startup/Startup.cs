﻿using System;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.AspNetCore.OData.Configuration;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Platform.Configuration;
using Platform.Identity;

using Abp.AspNetCore.SignalR.Hubs;
using Platform.Professions;
using System.Reflection;
using Platform.Web.Host.Filters;
using Abp.IdentityServer4;
using Platform.Authorization.Users;
using Platform.Authentication.JwtBearer;

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

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();
            services.AddOData();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
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


            //id4
            services.AddIdentityServer()
             // .AddDefaultUI(UIFramework.Bootstrap4)
              .AddDeveloperSigningCredential()
              .AddInMemoryIdentityResources(Config.GetIdentityResources())
              .AddInMemoryApiResources(Config.GetApiResources())
              .AddInMemoryClients(Config.GetClients())
              .AddAbpPersistedGrants<IAbpPersistedGrantDbContext>()
              .AddAbpIdentityServer<User>();

            services.AddAuthentication().AddIdentityServerAuthentication("IdentityBearer", options =>
            {
                options.Authority = "http://localhost:21021//";
                options.RequireHttpsMetadata = false;
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
            app.UseAbp(options => { options.UseAbpRequestLocalization = true; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseJwtTokenMiddleware("IdentityBearer");
            app.UseAuthentication();
            // app.UseIdentityServer();

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
                        .Expand() // Allow for the $expand Command
                        .OrderBy() // Allow for the $orderby Command
                        .Page() // Allow for the $top and $skip Commands
                        .Select();// Allow for the $select Command; 
                builder.EntitySet<Profession>("ProfessionTranslations").EntityType
                       .Filter() // Allow for the $filter Command
                       .Count() // Allow for the $count Command
                       .Expand() // Allow for the $expand Command
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
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "Platform API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Platform.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
        }
    }
}
