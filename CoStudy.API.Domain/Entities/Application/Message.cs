using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Message
    {
        public ObjectId Id { get; set; }
        public string AuthorId { get; set; }

        public bool IsReadByAuthor { get; set; }
        public bool IsReadByGuest { get; set; }

        public MediaContent Content { get; set; }

        /// <summary>
        /// Delete or not ?
        /// </summary>
        public ItemStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
