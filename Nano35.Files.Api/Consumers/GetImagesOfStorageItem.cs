using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Nano35.Contracts.chrono.Artifacts;
using Nano35.Contracts.files;
using Nano35.Contracts.Identity.Artifacts;
using Nano35.Files.Api.Services;

namespace Nano35.Files.Api.Consumers
{
    public class GetImagesOfStorageItemConsumer : 
        IConsumer<IGetImagesOfStorageItemRequestContract>
    {
        private readonly ApplicationContext _context;
        
        public GetImagesOfStorageItemConsumer(
            ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task Consume(
            ConsumeContext<IGetImagesOfStorageItemRequestContract> context)
        {
            try
            {
                var images = await _context
                    .ImagesOfStorageItems
                    .Where(e => e.StorageItemId == context.Message.StorageItemId)
                    .Select(e => e.NormalizedName)
                    .ToListAsync();
                await context.RespondAsync(new GetImagesOfStorageItemSuccessResultContract() {Images = images});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await context.RespondAsync(new GetImagesOfStorageItemErrorResultContract() {Message = e.Message});
            }
        }
    }
}