using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Eatster.Api.Extensions;
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
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;
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

            services.AddMvc()
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

            services.AddJwtConfiguration(Configuration);

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Eatster API";
                    document.Info.Description = "Api for Eatster, An Eating hub for youngsters.";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "Shubham Shukla",
                        Email = string.Empty,
                        Url = "https://twitter.com/LighghtCoder"
                    };
                };
                config.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token",
                    new SwaggerSecurityScheme
                    {
                        Type = SwaggerSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Copy 'Bearer ' + valid JWT token into field",
                        In = SwaggerSecurityApiKeyLocation.Header
                    }));

                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });

            services.AddAutoMapper();

            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
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
            app.UseMvc();
        }
    }
}