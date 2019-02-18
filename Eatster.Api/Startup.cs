using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Eatster.Api.Extensions;
using Eatster.Api.Filters;
using Eatster.Api.Infrastructure;
using Eatster.Application.Auth.Login.Command;
using Eatster.Application.Infrastructure;
using Eatster.Application.Login.Command;
using Eatster.Persistence.Data;
using Eatster.Persistence.Identity;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Eatster.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidation<,>));
            services.AddMediatR(typeof(LoginCommandHandler).GetTypeInfo().Assembly);

            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginCommandValidator>());

            services.AddDbContext<AppIdentityDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("Default"),
                                  i => i.MigrationsAssembly("Eatster.Persistence")));
            services.AddDbContext<AppDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("Default"),
                                  i => i.MigrationsAssembly("Eatster.Persistence")));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("fully permissive", configurePolicy => configurePolicy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });

            services.AddJwtConfiguration(Configuration);

            services.AddSwaggerConfig();

            services.AddAutoMapper();

            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseCors("fully permissive");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}