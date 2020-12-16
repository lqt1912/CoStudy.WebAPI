using Base;
using Examples.Models;

namespace Mongo.Entities.Demo.AuthorRepository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository() : base("Author")
        {

        }
    }
}
