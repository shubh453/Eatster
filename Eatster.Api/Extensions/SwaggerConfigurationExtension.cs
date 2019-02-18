using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace Eatster.Api.Extensions
{
    public static class SwaggerConfigurationExtension
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
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
        }
    }
}