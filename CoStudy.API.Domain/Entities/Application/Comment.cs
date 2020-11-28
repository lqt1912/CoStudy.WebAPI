using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
   public  class Comment
    {
        public ObjectId Id { get; set; }
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public ItemStatus Status { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }
    }
}
