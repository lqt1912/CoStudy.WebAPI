using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Post :Entity
    {
        public Post() : base()
        {
            Contents = new List<MediaContent>();
            Comments = new List<Comment>();
            Fields = new List<Field>();
        }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("upvote")]
        public int Upvote { get; set; }

        [BsonElement("downvote")]
        public int Downvote { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Nội dung 
        /// </summary>
        /// 
        [BsonElement("contents")]
        public  List<MediaContent> Contents { get; set; }


        /// <summary>
        /// Bình luận
        /// </summary>
        /// 
        [BsonElement("comments")]
        public  List<Comment> Comments { get; set; }

        /// <summary>
        /// Lĩnh vực của bài post
        /// </summary>
        /// 
        [BsonElement("fields")]
        public  List<Field> Fields { get; set; }
    }
}
