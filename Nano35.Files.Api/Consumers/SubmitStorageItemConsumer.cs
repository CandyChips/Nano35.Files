using System;
using System.Threading.Tasks;
using MassTransit;
using Nano35.Contracts.chrono.Artifacts;
using Nano35.Contracts.Identity.Artifacts;

namespace Nano35.Files.Api.Consumers
{
    public class SubmitStorageItemConsumer : IConsumer<ISubmitStorageItemRequestContract>
    {
        public async Task Consume(ConsumeContext<ISubmitStorageItemRequestContract> context)
        {
            throw new NotImplementedException();
        }
    }
}