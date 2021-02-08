using System.Threading.Tasks;

namespace Nano35.Files.Processor.Requests
{
    public interface IPipelineNode<TIn, TOut>
    {
        Task<TOut> Ask(TIn input);
    }
}