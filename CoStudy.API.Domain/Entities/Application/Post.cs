using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Post
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }

        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Nội dung 
        /// </summary>
        public virtual ICollection<MediaContent> Contents { get; set; }


        /// <summary>
        /// Bình luận
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Lĩnh vực của bài post
        /// </summary>
        public virtual ICollection<Field> Fields { get; set; }
    }
}
