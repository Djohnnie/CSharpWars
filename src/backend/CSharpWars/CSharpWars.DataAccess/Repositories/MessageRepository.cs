using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.Messages)
        {

        }
    }
}