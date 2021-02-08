using System.Threading.Tasks;
using Nano35.Contracts.files;

namespace Nano35.Files.Processor.Requests.CreateStorageImage
{
    public class CreateStorageItemImageValidatorErrorResult : 
        ICreateStorageItemImageErrorResultContract
    {
        public string Message { get; set; }
    }
    public class CreateStorageItemImageValidator :
        IPipelineNode<
            ICreateStorageItemImageRequestContract, 
            ICreateStorageItemImageResultContract>
    {        
        private readonly IPipelineNode<
            ICreateStorageItemImageRequestContract,
            ICreateStorageItemImageResultContract> _nextNode;

        public CreateStorageItemImageValidator(
            IPipelineNode<
                ICreateStorageItemImageRequestContract, 
                ICreateStorageItemImageResultContract> nextNode)
        {
            _nextNode = nextNode;
        }

        public async Task<ICreateStorageItemImageResultContract> Ask(
            ICreateStorageItemImageRequestContract input)
        {
            if (false)
            {
                return new CreateStorageItemImageValidatorErrorResult() 
                    {Message = "Ошибка валидации"};
            }
            return await _nextNode.Ask(input);
        }
    }
}