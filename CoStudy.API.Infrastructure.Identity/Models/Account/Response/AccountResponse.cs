﻿using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Response
{
    public class AccountResponse
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
    }
}
