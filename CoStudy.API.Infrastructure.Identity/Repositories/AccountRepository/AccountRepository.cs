using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        IConfiguration configuration;
        public AccountRepository(IConfiguration configuration) : base("account",configuration)
        {
            this.configuration = configuration;
        }
    }
}
