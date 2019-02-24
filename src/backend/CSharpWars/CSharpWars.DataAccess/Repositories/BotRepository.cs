using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class BotRepository : Repository<Bot>
    {
        public BotRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.Bots)
        {

        }
    }
}