using System.Threading.Tasks;

namespace CSharpWars.ScriptProcessor.Middleware.Interfaces
{
    public interface IProcessor
    {
        Task Go(ProcessingContext context);
    }
}