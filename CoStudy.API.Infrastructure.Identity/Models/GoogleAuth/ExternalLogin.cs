using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Models.GoogleAuth
{
    public class ExternalLogin : Entity
    {
        [BsonElement("login_provider")]
        public string LoginProvider { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
