using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class ScriptRepository : Repository<BotScript>
    {
        public ScriptRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.BotScripts)
        {

        }
    }
}