using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class PlayerRepository : Repository<Player>
    {
        public PlayerRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.Players)
        {

        }
    }
}