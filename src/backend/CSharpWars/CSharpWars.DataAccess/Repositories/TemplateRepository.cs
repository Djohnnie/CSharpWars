using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories
{
    public class TemplateRepository : Repository<Template>
    {
        public TemplateRepository(CSharpWarsDbContext dbContext) : base(dbContext, dbContext.Templates)
        {

        }
    }
}