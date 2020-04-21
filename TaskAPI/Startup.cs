using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskAPI.Common.Enums;
using TaskAPI.Common.Middlewares;
using TaskAPI.Common.Options; 
using TaskAPI.Data.DataContexts;
using TaskAPI.Data.Models;
using TaskAPI.Services;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Models.Outbound;
using TaskAPI.Services.Validators;

namespace TaskAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailerOptions>(Configuration.GetSection("Mailer"));
            services.AddTransient<IMailer, Mailer>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, GetUserResponse>()
                    .ForMember(m => m.FirstName, dest => dest.MapFrom(o => o.FirstName))
                    .ForMember(m => m.LastName, dest => dest.MapFrom(o => o.LastName))
                    .ForMember(m => m.Phone, dest => dest.MapFrom(o => o.Phone))
                    .ForMember(m => m.Email, dest => dest.MapFrom(o => o.Email))
                    .ForMember(m => m.UserId, dest => dest.MapFrom(o => o.UserId))
                ;

                cfg.CreateMap<CreateUserRequest, User>()
                    .ForMember(m => m.UserId, dest => dest.MapFrom(o => Guid.NewGuid()))
                    .ForMember(m => m.Password, dest => dest.MapFrom(o => o.Password))
                    .ForMember(m => m.Email, dest => dest.MapFrom(o => o.Email))
                    .ForMember(m => m.FirstName, dest => dest.MapFrom(o => o.FirstName))
                    .ForMember(m => m.LastName, dest => dest.MapFrom(o => o.LastName))
                    .ForMember(m => m.Address, dest => dest.MapFrom(o => o.Address))
                    .ForMember(m => m.Phone, dest => dest.MapFrom(o => o.Phone))
                    .ForMember(m => m.Sex, dest => dest.MapFrom(o => (Sex)o.Sex))
                    .ForMember(m => m.JwtToken, dest => dest.MapFrom(o => string.Empty))
                    .ForMember(m => m.PasswordRecoveryToken, dest => dest.MapFrom(o => string.Empty))
                    .ForMember(m => m.Assignments, dest => dest.MapFrom(o => new List<TaskAssignment>()))
                ;

                cfg.CreateMap<UpdateUserRequest, UpdateUser>()
                    .ForMember(m => m.FirstName, dest => dest.MapFrom(o => o.FirstName))
                    .ForMember(m => m.LastName, dest => dest.MapFrom(o => o.LastName))
                    .ForMember(m => m.Address, dest => dest.MapFrom(o => o.Address))
                    .ForMember(m => m.Phone, dest => dest.MapFrom(o => o.Phone))
                    .ForMember(m => m.Sex, dest => dest.MapFrom(o => (Sex)o.Sex))
                ;

                cfg.CreateMap<CreateTaskRequest, UserTask>()
                    .ForMember(m => m.TaskId, dest => dest.MapFrom(o => Guid.NewGuid()))
                    .ForMember(m => m.Done, dest => dest.MapFrom(o => false))
                    .ForMember(m => m.Name, dest => dest.MapFrom(o => o.Name))
                    .ForMember(m => m.TargetDate, dest => dest.MapFrom(o => o.TargetDate))
                    .ForMember(m => m.Priority, dest => dest.MapFrom(o => (Priority)o.Priority))
                    .ForMember(m => m.Assignments, dest => dest.MapFrom(o => new List<TaskAssignment>()))
                ;

                cfg.CreateMap<UpdateUserRequest, UpdateTask>();

                cfg.CreateMap<UserTask, GetTaskResponse>();

            });

            services.AddSingleton(config.CreateMapper());

            services.AddDbContext<DataContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("TaskAPI"),
                x => x.MigrationsAssembly("TaskAPI.Data")
            ));

            services.AddHttpClient();

            services.AddTransient<IRequestValidator, RequestValidator>();

            services.AddTransient<IIdentityProvider, IdentityProvider>();
            services.AddTransient<ILoginService, LoginService>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITasksService, TasksService>();

            services.Add(new ServiceDescriptor(typeof(IDataValidator), typeof(UserDataValidator), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IDataValidator), typeof(TaskDataValidator), ServiceLifetime.Transient));
            services.AddTransient<ICompositeDataValidator, CompositeDataValidator>();

            var identityServerOptions = new IdentityServerOptions();
            Configuration.Bind("IdentityServer", identityServerOptions);
            services.Configure<IdentityServerOptions>(Configuration.GetSection("IdentityServer"));

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityServerOptions.Host;
                    options.RequireHttpsMetadata = identityServerOptions.RequireHttps;
                    options.Audience = "api1";
                });

            services.AddControllers();
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
