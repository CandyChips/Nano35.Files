using System.Threading.Tasks;
using Nano35.Contracts.files;
using Nano35.Files.Processor.Services;

namespace Nano35.Files.Processor.Requests.CreateStorageImage
{
    public class CreateStorageItemImageSuccessResultContract : 
        ICreateStorageItemImageSuccessResultContract
    {
            
    }
    
    public class CreateStorageItemImageRequest :
        IPipelineNode<ICreateStorageItemImageRequestContract, ICreateStorageItemImageResultContract>
    {
        private readonly ApplicationContext _context;

        public CreateStorageItemImageRequest(
            ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ICreateStorageItemImageResultContract> Ask(
            ICreateStorageItemImageRequestContract input)
        {
            return new CreateStorageItemImageSuccessResultContract();
        }
    }
}