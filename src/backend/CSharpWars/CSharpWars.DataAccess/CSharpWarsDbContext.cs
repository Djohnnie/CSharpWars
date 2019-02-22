using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Model;
using Microsoft.EntityFrameworkCore;

namespace CSharpWars.DataAccess
{
    public class CSharpWarsDbContext : DbContext
    {
        private readonly IConfigurationHelper _configurationHelper;

        public DbSet<Player> Players { get; set; }

        public DbSet<Bot> Bots { get; set; }

        public CSharpWarsDbContext(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configurationHelper.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var playerEntity = modelBuilder.Entity<Player>();
            playerEntity.ToTable("PLAYERS").HasKey(x => x.Id).ForSqlServerIsClustered(false);
            playerEntity.Property(x => x.SysId).UseSqlServerIdentityColumn();
            playerEntity.HasIndex(x => x.SysId).ForSqlServerIsClustered();

            var botEntity = modelBuilder.Entity<Bot>();
            botEntity.ToTable("BOTS").HasKey(x => x.Id).ForSqlServerIsClustered(false);
            botEntity.Property(x => x.SysId).UseSqlServerIdentityColumn();
            botEntity.HasIndex(x => x.SysId).ForSqlServerIsClustered();
        }
    }
}