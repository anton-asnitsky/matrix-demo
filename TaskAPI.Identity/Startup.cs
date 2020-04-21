using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskAPI.Data.DataContexts;
using TaskAPI.Identity.Configuration;
using TaskAPI.Identity.Providers;
using Newtonsoft.Json.Serialization;
using TaskAPI.Common.Middlewares;

namespace TaskAPI.Identity
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddHttpClient();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TaskAPI")));

            var identityServerOptions = new Common.Options.IdentityServerOptions();
            Configuration.Bind("IdentityServer", identityServerOptions);

            var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), identityServerOptions.CertFolder, identityServerOptions.CertFile);

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources(identityServerOptions.Scope))
                .AddInMemoryClients(IdentityServerConfiguration.GetClients(identityServerOptions.ClientId, identityServerOptions.ClientSecret))
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                .AddProfileService<CustomProfileService>()
                .AddSigningCredential(new X509Certificate2(certificatePath, identityServerOptions.CertPassword));

            services.AddMvcCore().AddApiExplorer();

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
