using System.Threading.Tasks;
using CSharpWars.DtoModel;

namespace CSharpWars.Processor.Middleware.Interfaces
{
    public interface IBotProcessingFactory
    {
        Task Process(BotDto bot, ProcessingContext context);
    }
}