using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Image
    {
        public ObjectId Id { get; set; }

        public string Discription { get; set; }

        public string ImageUrl { get; set; }
        public string Base64String  { get; set; }

        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }

        public double CompressRatio { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
