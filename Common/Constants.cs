using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Constants
    {
        public static string IsActive = "is_active";

        public static string CreatedDate = "created_date";

        public static string ObjectIdCs = "object_id";

        public static string Status = "status";

        public static string IsDeleted = "is_deleted";

        public static class ExternalLoginConstants
        {
            public static string GOOGLE_EXTERNAL_PROVIDER = "GOOGLE";
        }

        public static class LevelConstants
        {
            public static string LEVEL_0_ID = "6031da2eba003751a1470d42";
        }

        public static class MessageTypeConstants
        {
            public static string TEXT_MESSAGE = "TEXT_MESSAGE";

            public static string IMAGE_MESSAGE = "IMAGE_MESSAGE";

            public static string ADD_MEMBER_MESSAGE = "ADD_MEMBER_MESSAGE";

            public static string POST_THUMBNAIL_MESSAGE = "POST_THUMBNAIL_MESSAGE";
        }

        public static class NotificationContentTypeConstant
        {
            public static string ADD_POST_NOTIFY = " đã thêm bài viết liên quan ";

            public static string UPVOTE_POST_NOTIFY = " đã upvote bài viết của  ";

            public static string DOWNVOTE_POST_NOTIFY = " đã downvote bài viết của ";

            public static string UPVOTE_COMMENT_NOTIFY = " đã upvote bình luận của ";

            public static string DOWNVOTE_COMMENT_NOTIFY = " đã downvote bình luận của ";

            public static string UPVOTE_REPLY_NOTIFY = " đã upvote trả lời của ";

            public static string DOWNVOTE_REPLY_NOTIFY = " đã downvote trả lời của ";

            public static string FOLLOW_NOTIFY =  " đã theo dõi ";

            public static string COMMENT_NOTIFY = " đã bình luận về bài viết của ";

            public static string REPLY_COMMENT_NOTIFY = " đã trả lời bình luận của ";
        }
        public static class PointAdded
        {
            public static int Default = 2;
            public static int Level1 = 1;
            public static int Level2 = 3;
            public static int Level3 = 5;
            public static int Level4 = 8;
            public static int Level5 = 15;
            public static int Upvote = 2;
            public static int Downvote = -2;
        }
        public enum CronTime
        {
            Secondly, 
            Minutely, 
            Hourly,
            Daily, 
            Monthly
        }

        public enum PostType
        {
           Question, 
           Sharing
        }
    }
}
