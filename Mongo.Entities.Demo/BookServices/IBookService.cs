using Examples.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Entities.Demo.BookServices
{
    public interface IBookService
    {
        Task CreateAsync(Book entity);


    }
}
