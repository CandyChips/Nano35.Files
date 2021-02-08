using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Nano35.Contracts;
using Nano35.Contracts.Identity.Artifacts;
using Nano35.Files.Processor.Contracts;

namespace Nano35.Files.Processor.Configurators
{
    public class MassTransitConfiguration : 
        IConfigurationOfService
    {
        public void AddToServices(
            IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri($"{ContractBase.RabbitMqLocation}/"), h =>
                    {
                        h.Username(ContractBase.RabbitMqUsername);
                        h.Password(ContractBase.RabbitMqPassword);
                    });
                    
                    cfg.ReceiveEndpoint("ICreateStorageItemImageRequestContract", e =>
                    {
                        e.Consumer<CreateStorageItemImage>(provider);
                    });
                }));
                x.AddConsumer<CreateStorageItemImage>();
                
                x.AddRequestClient<IGetUserByIdRequestContract>(new Uri($"{ContractBase.RabbitMqLocation}/IGetUserByIdRequestContract"));
            });
            services.AddMassTransitHostedService();
        }
    }
}