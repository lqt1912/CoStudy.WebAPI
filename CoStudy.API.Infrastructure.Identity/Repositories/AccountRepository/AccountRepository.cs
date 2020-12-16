using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository() : base("account")
        {

        }
    }
}
