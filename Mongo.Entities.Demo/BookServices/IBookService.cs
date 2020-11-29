using Examples.Models;
using System.Threading.Tasks;

namespace Mongo.Entities.Demo.BookServices
{
    public interface IBookService
    {
        Task CreateAsync(Book entity);


    }
}
