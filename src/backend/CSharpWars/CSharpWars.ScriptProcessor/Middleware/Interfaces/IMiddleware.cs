using System.Threading.Tasks;

namespace CSharpWars.ScriptProcessor.Middleware.Interfaces
{
    public interface IMiddleware
    {
        Task Process();
    }
}