using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nano35.Contracts.files;
using Nano35.Contracts.Storage.Artifacts;
using Nano35.Files.Processor.Requests.CreateStorageImage;
using Nano35.Files.Processor.Services;

namespace Nano35.Files.Processor.Contracts
{
    public class CreateStorageItemImage : 
        IConsumer<ICreateStorageItemImageRequestContract>
    {
        private readonly IServiceProvider _services;
        
        public CreateStorageItemImage(
            IServiceProvider services)
        {
            _services = services;
        }
        public async Task Consume(
            ConsumeContext<ICreateStorageItemImageRequestContract> context)
        {
            // Setup configuration of pipeline
            var dbContext = (ApplicationContext) _services.GetService(typeof(ApplicationContext));
            var logger = (ILogger<CreateStorageItemImageLogger>) _services.GetService(typeof(ILogger<CreateStorageItemImageLogger>));

            // Explore message of request
            var message = context.Message;

            // Send request to pipeline
            var result =
                await new CreateStorageItemImageLogger(logger,
                new CreateStorageItemImageValidator(
                    new CreateStorageItemImageRequest(dbContext))
                ).Ask(message);
            
            // Check response of create storage item image request
            switch (result)
            {
                case ICreateStorageItemImageSuccessResultContract:
                    await context.RespondAsync<ICreateStorageItemImageSuccessResultContract>(result);
                    break;
                case ICreateStorageItemImageErrorResultContract:
                    await context.RespondAsync<ICreateStorageItemImageErrorResultContract>(result);
                    break;
            }
        }
    }
}