using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
  public class Address
    {
        public ObjectId Id { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Detail { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
