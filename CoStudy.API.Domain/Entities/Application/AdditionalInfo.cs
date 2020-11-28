using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class AdditionalInfo
    {
        public ObjectId Id { get; set; }

        public InfoType InfoType  { get; set; }
        public string InfoValue { get; set;}
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
