using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nano35.Contracts.files;

namespace Nano35.Files.Processor.Requests.CreateStorageImage
{
    public class CreateStorageItemImageLogger :
        IPipelineNode<ICreateStorageItemImageRequestContract, ICreateStorageItemImageResultContract>
    {
        private readonly ILogger<CreateStorageItemImageLogger> _logger;
        private readonly IPipelineNode<ICreateStorageItemImageRequestContract, ICreateStorageItemImageResultContract> _nextNode;

        public CreateStorageItemImageLogger(
            ILogger<CreateStorageItemImageLogger> logger,
            IPipelineNode<ICreateStorageItemImageRequestContract, ICreateStorageItemImageResultContract> nextNode)
        {
            _logger = logger;
            _nextNode = nextNode;
        }

        public async Task<ICreateStorageItemImageResultContract> Ask(
            ICreateStorageItemImageRequestContract input)
        {
            _logger.LogInformation($"Create article logger starts on: {DateTime.Now}");
            var result = await _nextNode.Ask(input);
            _logger.LogInformation($"Create article logger ends on: {DateTime.Now}");
            return result;
        }
    }
}