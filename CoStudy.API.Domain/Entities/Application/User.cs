using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
   public  class User
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Address Address { get; set; }

        public Image Avatar { get; set; }
        public ItemStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Noftication> Noftications { get; set; }
        public virtual ICollection<string> Followers { get; set; }


        public virtual ICollection<string> Following { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; }

        /// <summary>
        /// Sở trường
        /// </summary>
        public virtual ICollection<Field> Fortes { get; set; }
    }
}
