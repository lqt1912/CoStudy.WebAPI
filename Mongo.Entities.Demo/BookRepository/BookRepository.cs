using Base;
using Examples.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.Entities.Demo.BookRepository1
{
    public class BookRepository : BaseRepository<Book>,IBookRepository
    {
        public BookRepository() : base("Book")
        {

        }
    }
}
