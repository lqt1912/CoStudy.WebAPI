﻿using Examples.Models;
using MongoDB.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Mongo.Entities.Demo.BookRepository1;
using Mongo.Entities.Demo.BookServices;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Mongo.Entities.Demo
{
    public static class Program
    {
        private static async Task Main(string[] _)
        {


            //SAVING
            var book1 = new Book { Title = "Sách test new " };

            var author = new Author()
            {
                Name = "Lê Quốc Thắng",
                PhoneNumber = "012893723"
             };
            var review = new Review()
            {
                Stars = 3,
                Reviewer = "The Reviewers "
            };
            var review2 = new Review()
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

            var bookRepo = new BookService();

            await bookRepo.CreateAsync(book1);
            Console.WriteLine("Add xong rồi đó :)) ");

            var author3 = new Author()
            {
                Name = "Lê Quốc Thắng Added ",
                PhoneNumber = "012893723"
            };
            await bookRepo.AddAuthor(ObjectId.Parse("5fb61b3aec3a441caf868da6"), author3);
        }
    }
}
