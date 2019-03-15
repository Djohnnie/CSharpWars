using System.Threading.Tasks;

namespace CSharpWars.ScriptProcessor.Middleware.Interfaces
{
    public interface IPreprocessor
    {
        Task Go(ProcessingContext context);
    }
}