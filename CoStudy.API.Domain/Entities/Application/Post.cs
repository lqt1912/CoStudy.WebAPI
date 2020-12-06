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
            StringContents = new List<PostContent>();
            MediaContents = new List<Image>();
            Comments = new List<Comment>();
            Fields = new List<Field>();
        }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("upvote")]
        public int Upvote { get; set; }

        [BsonElement("downvote")]
        public int Downvote { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// String content of posts
        /// </summary>
        [BsonElement("string_contents")]
        public  List<PostContent> StringContents { get; set; }


        /// <summary>
        /// Media content of post
        /// </summary>
        [BsonElement("image_contents")]
        public List<Image> MediaContents { get; set; }

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
