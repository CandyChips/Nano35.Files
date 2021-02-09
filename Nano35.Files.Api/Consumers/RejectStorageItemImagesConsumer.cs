using System;
using System.Threading.Tasks;
using MassTransit;
using Nano35.Contracts.chrono.Artifacts;
using Nano35.Contracts.Identity.Artifacts;

namespace Nano35.Files.Api.Consumers
{
    public class RejectStorageItemImagesConsumer : 
        IConsumer<IRejectStorageItemImagesRequestContract>
    {
        public RejectStorageItemImagesConsumer()
        {
            
        }
        
        public async Task Consume(
            ConsumeContext<IRejectStorageItemImagesRequestContract> context)
        {
            throw new NotImplementedException();
        }
    }
}