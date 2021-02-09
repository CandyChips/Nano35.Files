using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Nano35.Contracts;
using Nano35.Files.Api.Consumers;

namespace Nano35.Files.Api.Configurators
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
                    
                    cfg.ReceiveEndpoint("IRejectStorageItemImagesRequestContract", e =>
                    {
                        e.Consumer<RejectStorageItemImagesConsumer>(provider);
                    });
                    
                    cfg.ReceiveEndpoint("ISubmitStorageItemRequestContract", e =>
                    {
                        e.Consumer<SubmitStorageItemConsumer>(provider);
                    });
                    
                }));
                x.AddConsumer<RejectStorageItemImagesConsumer>();
                x.AddConsumer<SubmitStorageItemConsumer>();
            });
            services.AddMassTransitHostedService();
        }
    }
}