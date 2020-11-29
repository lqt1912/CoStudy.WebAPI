using Base;
using Examples.Models;

namespace Mongo.Entities.Demo.BookRepository1
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository() : base("Book")
        {

        }
    }
}
