using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nano35.Contracts;

namespace Nano35.Files.Api.Configurators
{
    public class SwaggerConfiguration : 
        IConfigurationOfService
    {
        public void AddToServices(
            IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Storage.Projection API",
                    Description = "A entrypoint to instance microservice",
                    Contact = new OpenApiContact
                    {
                        Name = "Guihub folder",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/CandyChips/Nano35.Storage.Projection")
                    }
                });
            });
        }
    }
}