using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
   public  class Conversation
    {
        public ObjectId Id { get; set; }

        public string HostId { get; set;  }
        public string GuestId { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public ItemStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
