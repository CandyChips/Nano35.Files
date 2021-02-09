using Microsoft.Extensions.DependencyInjection;
using Nano35.Contracts;

namespace Nano35.Files.Api.Configurators
{
    public class CorsConfiguration : 
        IConfigurationOfService
    {
        public void AddToServices(
            IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Cors", builder => 
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
        }
    }
}