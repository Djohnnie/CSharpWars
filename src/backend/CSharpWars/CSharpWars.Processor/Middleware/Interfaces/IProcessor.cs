using System.Threading.Tasks;

namespace CSharpWars.Processor.Middleware.Interfaces
{
    public interface IProcessor
    {
        Task Go(ProcessingContext context);
    }
}