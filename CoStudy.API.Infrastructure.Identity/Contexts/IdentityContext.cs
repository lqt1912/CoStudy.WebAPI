using CoStudy.API.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Infrastructure.Identity.Contexts
{
    public class IdentityContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        private readonly IConfiguration Configuration;

        public IdentityContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("IdentityDatabase"));
        }
    }
}
