using System.Threading.Tasks;

namespace CSharpWars.ScriptProcessor.Middleware.Interfaces
{
    public interface IPostprocessor
    {
        Task Go(ProcessingContext context);
    }
}