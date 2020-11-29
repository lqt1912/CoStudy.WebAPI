using Examples.Models;
using Mongo.Entities.Demo.BookRepository1;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Mongo.Entities.Demo.BookServices
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService()
        {
            _bookRepository = new BookRepository();
        }

        public async Task CreateAsync(Book entity)
        {

            await _bookRepository.AddAsync(entity);
        }

        public async Task AddAuthor(ObjectId id, Author entity)
        {
            var currentBook = await _bookRepository.GetByIdAsync(id);
            currentBook.Authors.Add(entity);
            await _bookRepository.UpdateAsync(currentBook, id);
        }
    }
}
