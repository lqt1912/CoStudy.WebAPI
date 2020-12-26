using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    public class PostService : IPostService
    {
        IHttpContextAccessor httpContextAccessor;
        IConfiguration configuration;
        IUserRepository userRepository;
        IPostRepository postRepository;
        ICommentRepository commentRepository;
        IReplyCommentRepository replyCommentRepository;
        IFieldRepository fieldRepository;
        IFollowRepository followRepository;
        IUpVoteRepository upVoteRepository;
        IDownVoteRepository downVoteRepository;

        public PostService(IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration, 
            IUserRepository userRepository,
            IPostRepository postRepository, 
            ICommentRepository commentRepository, 
            IReplyCommentRepository replyCommentRepository, 
            IFieldRepository fieldRepository,
            IFollowRepository followRepository, 
            IUpVoteRepository upVoteRepository, 
            IDownVoteRepository downVoteRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.fieldRepository = fieldRepository;
            this.followRepository = followRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
        }

        public async Task<AddCommentResponse> AddComment(AddCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
                comment.AuthorAvatar = currentUser.AvatarHash;
                comment.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

                currentPost.CommentCount++;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                await commentRepository.AddAsync(comment);
                //Update again
                return PostAdapter.ToResponse(comment, request.PostId);
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<AddMediaResponse> AddMedia(AddMediaRequest request)
        {
            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var image = PostAdapter.FromRequest(request, httpContextAccessor);
                currentPost.MediaContents.Add(image);
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                return PostAdapter.ToResponse(image, currentPost.Id.ToString());
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            currentUser.PostCount++;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            var post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();
            post.AuthorAvatar = currentUser.AvatarHash;

            post.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

            foreach (var fieldId in request.Fields)
            {
                var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
                if (field != null)
                    post.Fields.Add(field);
            }

            await postRepository.AddAsync(post);

            return PostAdapter.ToResponse(post, currentUser.Id.ToString());
        }

        public async Task<string> DeleteComment(string commentId)
        {
            var currentComment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            if (currentComment != null)
            {
                currentComment.Status = ItemStatus.Deleted;
                await commentRepository.UpdateAsync(currentComment, currentComment.Id);

                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(currentComment.PostId));
                currentPost.CommentCount--;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                return "Xóa bình luận thành công";
            }
            else throw new Exception("Comment không tồn tại hoặc đã bị xóa");
        }
        public async Task<string> DeleteReply(string replyId)
        {
            var currentReply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(replyId));
            if (currentReply != null)
            {
                currentReply.Status = ItemStatus.Deleted;
                await replyCommentRepository.UpdateAsync(currentReply, currentReply.Id);
                return "Xóa câu trả lời thành công";
            }
            else throw new Exception("Câu rả lời không tồn tại hoặc đã bị xóa");
        }
        public List<Comment> GetCommentByPostId(string postId)
        {
            var comments = commentRepository.GetAll().Where(x => x.PostId == postId && x.Status == ItemStatus.Active).ToList();
            if (comments != null)
                return comments;
            return null;
        }

        public async Task<GetPostByIdResponse> GetPostById(string postId)
        {
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
            if (post == null || post.Status != ItemStatus.Active)
                throw new Exception("Không tìm thấy bài viết ");
            return PostAdapter.ToResponse(post);
        }

        public async Task<IEnumerable<Post>> GetPostByUserId(string userId, int skip, int count)
        {
            try
            {
                var filter = Builders<Post>.Filter;
                var match = filter.Eq("author_id", userId) & filter.Eq("status", ItemStatus.Active);

                return (await postRepository.FindListAsync(match)).Skip(skip).Take(count);
            }
            catch (Exception)
            {
                throw new Exception("Người dùng chưa có bài viết nào");
            }


        }

        public async Task<IEnumerable<Post>> GetPostTimelineAsync(int skip, int count)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var findFilter = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);

            var listFollow = await followRepository.FindListAsync(findFilter);

            var listAuthor = new List<string>();
            listAuthor.Add(currentUser.Id.ToString());
            foreach (var item in listFollow)
                listAuthor.Add(item.ToId);

            var result = new List<Post>();

            foreach (var author in listAuthor)
            {
                var builder = Builders<Post>.Filter;
                var postFindFilter = builder.Eq("author_id", author) & builder.Eq("status", ItemStatus.Active);
                result.AddRange(await postRepository.FindListAsync(postFindFilter));
            }

            return result.Skip(skip).Take(count).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<ReplyComment> GetReplyCommentByCommentId(string commentId)
        {
            var comments = replyCommentRepository.GetAll().Where(x => x.ParentId == commentId && x.Status == ItemStatus.Active).ToList();
            if (comments != null)
                return comments;
            return null;
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                await replyCommentRepository.AddAsync(replyComment);
                comment.RepliesCount++;
                await commentRepository.UpdateAsync(comment, comment.Id);
                return PostAdapter.ToResponseReply(replyComment);
            }
            else throw new Exception("Bình luận đã bị xóa");

        }

        public async Task SyncComment()
        {
            try
            {
                var posts = postRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (var post in posts)
                {
                    var latestComments = commentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.PostId == post.Id.ToString() && x.Status == ItemStatus.Active);
                    if (latestComments.Count() > 3)
                        latestComments = latestComments.Take(3);
                    post.Comments.Clear();
                    post.Comments.AddRange(latestComments);
                    await postRepository.UpdateAsync(post, post.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
                return;
            }
        }

        public async Task SyncReply()
        {
            try
            {
                var comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (var comment in comments)
                {
                    var latestReplies = replyCommentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.ParentId == comment.Id.ToString() && x.Status == ItemStatus.Active);
                    if (latestReplies.Count() > 3)
                        latestReplies = latestReplies.Take(3);
                    comment.Replies.Clear();
                    comment.Replies.AddRange(latestReplies);
                    await commentRepository.UpdateAsync(comment, comment.Id);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<string> Upvote(string postId)
        {
            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                //Chưa unlike
                if (!currentUser.PostUpvote.Contains(postId))
                {

                    if (!currentUser.PostDownvote.Contains(postId))
                    {

                        currentPost.Upvote++;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);

                        currentUser.PostUpvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    }
                    else if (currentUser.PostDownvote.Contains(postId))
                    {
                        currentPost.Downvote--;
                        currentPost.Upvote++;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);

                        currentUser.PostDownvote.Remove(postId);
                        currentUser.PostUpvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    }
                }
                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }
        public async Task<string> Downvote(string postId)
        {

            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                //chưa like
                if (!currentUser.PostDownvote.Contains(postId))
                {
                    if (!currentUser.PostUpvote.Contains(postId))
                    {
                        currentPost.Downvote++;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);
                        currentUser.PostDownvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);

                    }
                    else if (currentUser.PostUpvote.Contains(postId))
                    {
                        currentPost.Downvote++;
                        currentPost.Upvote--;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);

                        currentUser.PostUpvote.Remove(postId);
                        currentUser.PostDownvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    }
                }
                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }

        public async Task<Post> UpdatePost(UpdatePostRequest request)
        {
            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            if (currentPost != null)
            {
                currentPost.StringContents = request.StringContents;
                currentPost.MediaContents = request.MediaContents;
                currentPost.Title = request.Title;
                currentPost.Fields = request.Fields;
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
            }
            return currentPost;
        }

        public async Task<Post> SavePost(string id)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            if (post != null)
            {
                if (!currentUser.PostSaved.Contains(id))
                {
                    currentUser.PostSaved.Add(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    return post;
                }
                else
                {
                    currentUser.PostSaved.Remove(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    return post;
                }
            }
            else throw new Exception("Bài viết không tồn tại hoặc đã bị xóa. ");

        }

        public async Task<List<Post>> GetSavedPost(int skip, int count)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<Post> result = new List<Post>();
            foreach (var postId in currentUser.PostSaved)
            {
                var post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
                if (post != null)
                    result.Add(post);
            }
            return result.Skip(skip).Take(count).ToList();
        }

        public async Task<IEnumerable<Post>> Filter(FilterRequest filterRequest)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var findFilter = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);

            var listFollow = await followRepository.FindListAsync(findFilter);

            var listAuthor = new List<string>();

            foreach (var item in listFollow)
                listAuthor.Add(item.ToId);
            listAuthor.Add(currentUser.OId);
            var timelines = new List<Post>();

            foreach (var author in listAuthor)
            {
                var builder = Builders<Post>.Filter;
                var postFindFilter = builder.Eq("author_id", author) & builder.Eq("status", ItemStatus.Active);
                timelines.AddRange(await postRepository.FindListAsync(postFindFilter));
            }

            var queryable = timelines.AsQueryable();

            if (!String.IsNullOrEmpty(filterRequest.KeyWord))
                queryable = queryable.Where(x => x.Title.ToLower().Contains(filterRequest.KeyWord.ToLower()));
            if (filterRequest.FromDate != null)
                queryable = queryable.Where(x => x.CreatedDate >= filterRequest.FromDate);
            if (filterRequest.ToDate != null)
                queryable = queryable.Where(x => x.CreatedDate <= filterRequest.ToDate);
            if (!String.IsNullOrEmpty(filterRequest.Field))
            {
                var field = fieldRepository.GetById(ObjectId.Parse(filterRequest.Field));
                queryable = queryable.Where(x => x.Fields.Contains(field));
            }

            switch (filterRequest.OrderBy)
            {
                case PostOrder.CreatedDate:
                    {
                        if (filterRequest.OrderType == OrderType.Ascending)
                            queryable = queryable.OrderBy(x => x.CreatedDate);
                        else queryable = queryable.OrderByDescending(x => x.CreatedDate);
                        break;
                    }
                case PostOrder.Comment:
                    {
                        if (filterRequest.OrderType == OrderType.Ascending)
                            queryable = queryable.OrderBy(x => x.CreatedDate);
                        else queryable = queryable.OrderByDescending(x => x.CreatedDate);
                        break;
                    }
                case PostOrder.Upvote:
                    {
                        if (filterRequest.OrderType == OrderType.Ascending)
                            queryable = queryable.OrderBy(x => x.Upvote);
                        else queryable = queryable.OrderByDescending(x => x.Upvote);
                        break;
                    }
                default:
                    break;
            }
            if (filterRequest.Skip.HasValue && filterRequest.Count.HasValue)
                queryable = queryable.Skip(filterRequest.Skip.Value).Take(filterRequest.Count.Value);
            return queryable.ToList();

        }

        public async Task<string> UpvoteComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var builder = Builders<DownVote>.Filter;

            var downvoteFinder = builder.Eq("object_vote_id", commentId) & builder.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);
            if (existDownvote != null)
                await downVoteRepository.DeleteAsync(existDownvote.Id);

            var upvotebuilder = Builders<UpVote>.Filter;
            var upvoteFinder = upvotebuilder.Eq("object_vote_id", commentId) & upvotebuilder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(upvoteFinder);

            if (existUpvote == null)
            {
                var upvote = new UpVote()
                {
                    UpVoteBy = currentUser.OId,
                    ObjectVoteId = commentId,
                };

                await upVoteRepository.AddAsync(upvote);
                return "Upvote thành công";
            }

            else return "Bạn đã upvote rồi";
        }

        public async Task<string> DownvoteComment(string commentId)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var builder = Builders<UpVote>.Filter;
            var finder = builder.Eq("object_vote_id", commentId) & builder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(finder);

            if (existUpvote != null)
                await upVoteRepository.DeleteAsync(existUpvote.Id);


            var builderDownVote = Builders<DownVote>.Filter;

            var downvoteFinder = builderDownVote.Eq("object_vote_id", commentId) & builderDownVote.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);

            if (existDownvote == null)
            {
                var downVote = new DownVote()
                {
                    DownVoteBy = currentUser.OId,
                    ObjectVoteId = commentId,
                };

                await downVoteRepository.AddAsync(downVote);
                return "Downvote thành công";
            }
            else return "Bạn đã downvote rồi";
        }

        public async Task SyncVote()
        {
            try
            {
                var comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
                foreach (var comment in comments)
                {
                    var upvoteBuilder = Builders<UpVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.UpvoteCount = (await upVoteRepository.FindListAsync(upvoteBuilder)).Count;
                    await commentRepository.UpdateAsync(comment, comment.Id);

                    var downVoteBuilder = Builders<DownVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.DownvoteCount = (await downVoteRepository.FindListAsync(downVoteBuilder)).Count;
                    await commentRepository.UpdateAsync(comment, comment.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        public async Task SyncReplyVote()
        {
            try
            {
                var comments = replyCommentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
                foreach (var comment in comments)
                {
                    var upvoteBuilder = Builders<UpVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.UpvoteCount = (await upVoteRepository.FindListAsync(upvoteBuilder)).Count;
                    await replyCommentRepository.UpdateAsync(comment, comment.Id);

                    var downVoteBuilder = Builders<DownVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.DownvoteCount = (await downVoteRepository.FindListAsync(downVoteBuilder)).Count;
                    await replyCommentRepository.UpdateAsync(comment, comment.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

    }
}
