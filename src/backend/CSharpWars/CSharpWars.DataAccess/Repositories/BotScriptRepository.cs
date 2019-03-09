using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class BotScriptRepository : Repository<Bot, BotScript>
    {
        public BotScriptRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.Bots, dbContext.BotScripts)
        {

        }
    }
}