using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Noftication
    {
        public ObjectId Id { get; set; }
        public MediaContent Content { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


        public ItemStatus Status { get; set; }
    }
}
