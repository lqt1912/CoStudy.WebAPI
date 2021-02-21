using Examples.Models;
using Mongo.Entities.Demo.BookRepository1;
using Mongo.Entities.Demo.BookServices;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Mongo.Entities.Demo
{
    public static class Program
    {
        private static async Task Main(string[] _)
        {


            //SAVING
            Book book1 = new Book { Title = "Sách test new " };

            Author author = new Author()
            {
                Name = "Lê Quốc Thắng",
                PhoneNumber = "012893723"
            };
            Review review = new Review()
            {
                Stars = 3,
                Reviewer = "The Reviewers "
            };
            Review review2 = new Review()
            {
                Stars = 5,
                Reviewer = "The Reviewers next Gen "
            };
            author.Reviews.Add(review);
            author.Reviews.Add(review2);
            author.Reviews.Add(review);
            author.Reviews.Add(review2);

            book1.Authors.Add(author);
            book1.Authors.Add(author);
            book1.Authors.Add(author);

            BookService bookRepo = new BookService();

            await bookRepo.CreateAsync(book1);
            Console.WriteLine("Add xong rồi đó :)) ");

            Author author3 = new Author()
            {
                Name = "Lê Quốc Thắng Added ",
                PhoneNumber = "012893723"
            };
            await bookRepo.AddAuthor(ObjectId.Parse("5fb61b3aec3a441caf868da6"), author3);

            MongoDB.Driver.IMongoCollection<Book> bookCollection = new CustomMongoClient().GetDatabase().GetCollection<Book>("Book");
            System.Linq.IQueryable<Book> bookRepository = new BookRepository().GetAll();

        }
    }
}
