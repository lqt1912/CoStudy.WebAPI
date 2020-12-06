using Base;
using Examples.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.Entities.Demo.AuthorRepository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository() : base("Author")
        {

        }
    }
}
