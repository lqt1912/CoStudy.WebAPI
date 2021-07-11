using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Noftication : Entity
    {
        public Noftication() : base()
        {
        }

        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("owner_id")]
        public string OwnerId { get; set; }

        [BsonElement("receiver_id")]
        public string ReceiverId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("object_id")]
        public string  ObjectId { get; set; }

        [BsonElement("object_type")]
        public ObjectNotificationType ObjectType { get; set; }

        [BsonElement("object_thumbnail")]
        public string  ObjectThumbnail { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [BsonElement("status")]
        public ItemStatus Status { get; set; } = ItemStatus.Active;

        [BsonElement("is_read")]
        public bool? IsRead { get; set; } = false;

    }

    public  class NotificationContent
    {
        public static Triple<string, string, ObjectNotificationType> AddPostNotification 
            = new Triple<string, string, ObjectNotificationType>("ADD_POST_NOTIFY", ", người bạn theo dõi đã thêm một bài viết về lĩnh vực bạn quan tâm. ", ObjectNotificationType.Post);

        public static Triple<string, string, ObjectNotificationType> UpvotePostNotification
            = new Triple<string, string, ObjectNotificationType>("UPVOTE_POST_NOTIFY", "đã UP bài viết của ", ObjectNotificationType.Post);

        public static Triple<string, string, ObjectNotificationType> DownvotePostNotification
            = new Triple<string, string, ObjectNotificationType>("DOWNVOTE_POST_NOTIFY", "đã DOWN bài viết của ", ObjectNotificationType.Post);

        public static Triple<string, string, ObjectNotificationType> UpvoteCommentNotification
            = new Triple<string, string, ObjectNotificationType>("UPVOTE_COMMENT_NOTIFY", "đã UP bình luận của", ObjectNotificationType.Comment);

        public static Triple<string, string, ObjectNotificationType> DownvoteCommentNotification
            = new Triple<string, string, ObjectNotificationType>("DOWNVOTE_COMMENT_NOTIFY", "đã DOWN bình luận của", ObjectNotificationType.Comment);

        public static Triple<string, string, ObjectNotificationType> UpvoteReplyNotification
         = new Triple<string, string, ObjectNotificationType>("UPVOTE_REPLY_NOTIFY", "đã UP phản hồi của", ObjectNotificationType.Reply);

        public static Triple<string, string, ObjectNotificationType> DownvoteReplyNotification
            = new Triple<string, string, ObjectNotificationType>("DOWNVOTE_REPLY_NOTIFY", "đã DOWN phản hồi của", ObjectNotificationType.Reply);

        public static Triple<string, string, ObjectNotificationType> FollowNotification
            = new Triple<string, string, ObjectNotificationType>("FOLLOW_NOTIFY", "đã theo dõi", ObjectNotificationType.User);

        public static Triple<string, string, ObjectNotificationType> CommentNotification
            = new Triple<string, string, ObjectNotificationType>("COMMENT_NOTIFY", "đã bình luận về bài viết của ", ObjectNotificationType.Post);

        public static Triple<string, string, ObjectNotificationType> ReplyCommentNotification
           = new Triple<string, string, ObjectNotificationType>("REPLY_COMMENT_NOTIFY", "đã trả lời bình luận của ", ObjectNotificationType.Comment);

        public static Triple<string, string, ObjectNotificationType> ApprovePostReportNotification
            = new Triple<string, string, ObjectNotificationType>("APPROVE_POST_REPORT", "Bài viết của bạn đã bị xóa vì vi phạm tiêu chuẩn của chúng tôi. ", ObjectNotificationType.Other);

        public static Triple<string, string, ObjectNotificationType> ApproveCommentReportNotification
          = new Triple<string, string, ObjectNotificationType>("APPROVE_COMMENT_REPORT", "Bình luận của bạn đã bị xóa vì vi phạm tiêu chuẩn của chúng tôi. ", ObjectNotificationType.Other);
       
        public static Triple<string, string, ObjectNotificationType> ApproveReplyReportNotification
          = new Triple<string, string, ObjectNotificationType>("APPROVE_REPLY_REPORT", "Phản hồi của bạn đã bị xóa vì vi phạm tiêu chuẩn của chúng tôi. ", ObjectNotificationType.Other);

        public static Triple<string, string, ObjectNotificationType> PostReportNotification
            = new Triple<string, string, ObjectNotificationType>("POST_REPORT", "Bài viết của bạn đã bị báo cáo. ", ObjectNotificationType.Post);
        
        public static Triple<string, string, ObjectNotificationType> CommentReportNotification
            = new Triple<string, string, ObjectNotificationType>("COMMENT_REPORT", "Bình luận của bạn đã bị báo cáo. ", ObjectNotificationType.Comment);
       
        public static Triple<string, string, ObjectNotificationType> ReplyReportNotification
            = new Triple<string, string, ObjectNotificationType>("REPLY_REPORT", "Bài viết của bạn đã bị báo cáo. ", ObjectNotificationType.Reply);
    }

    public enum ObjectNotificationType
    {
        Post,
        Comment,
        Reply,
        User,
        Other
    }

    public struct Triple<a, b, c>
    {
        public a Item1;
        public b Item2;
        public c Item3;

        public Triple(a _item1, b _item2, c _item3)
        {
            Item1 = _item1;
            Item2 = _item2;
            Item3 = _item3;
        }
    }
}
