using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    public class User :Entity
    {
        [BsonElement("first_name")]
        public string FirstName { get; set; }

        [BsonElement("last_name")]
        public string LastName { get; set; }

        [BsonElement("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("avatar")]
        public Image Avatar { get; set; }

        [BsonElement("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }


        [BsonElement("posts")]
        public  List<Post> Posts { get; set; }


        [BsonElement("noftications")]
        public  List<Noftication> Noftications { get; set; }

        [BsonElement("followers")]
        public  List<string> Followers { get; set; }

        [BsonElement("followings")]
        public  List<string> Following { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// 
        [BsonElement("additional_infos")]
        public  List<AdditionalInfo> AdditionalInfos { get; set; }

        /// <summary>
        /// Sở trường
        /// </summary>
        /// 
        [BsonElement("fortes")]
        public  List<Field> Fortes { get; set; }

        public User() : base()
        {
            Avatar = new Image();
            Posts = new List<Post>();
            Noftications = new List<Noftication>();
            Followers = new List<string>();
            Following = new List<string>();
            AdditionalInfos = new List<AdditionalInfo>();
        }
    }
}
