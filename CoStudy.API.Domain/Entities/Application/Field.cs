using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
   public class Field
    {
        public ObjectId Id { get; set; }
        public string Value { get; set; }
        public Image ThumbnailImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


    }
}
