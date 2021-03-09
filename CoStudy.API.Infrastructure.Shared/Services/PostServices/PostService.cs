using CoStudy.API.Application.FCM;
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
    /// <summary>
    /// The Post Service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.PostServices.IPostService" />
    public class PostService : IPostService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;
        /// <summary>
        /// The comment repository
        /// </summary>
        ICommentRepository commentRepository;
        /// <summary>
        /// The reply comment repository
        /// </summary>
        IReplyCommentRepository replyCommentRepository;
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;
        /// <summary>
        /// The follow repository
        /// </summary>
        IFollowRepository followRepository;
        /// <summary>
        /// Up vote repository
        /// </summary>
        IUpVoteRepository upVoteRepository;
        /// <summary>
        /// Down vote repository
        /// </summary>
        IDownVoteRepository downVoteRepository;
        /// <summary>
        /// The client group repository
        /// </summary>
        IClientGroupRepository clientGroupRepository;
        /// <summary>
        /// The FCM repository
        /// </summary>
        IFcmRepository fcmRepository;
        /// <summary>
        /// The noftication repository
        /// </summary>
        INofticationRepository nofticationRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="commentRepository">The comment repository.</param>
        /// <param name="replyCommentRepository">The reply comment repository.</param>
        /// <param name="fieldRepository">The field repository.</param>
        /// <param name="followRepository">The follow repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="fcmRepository">The FCM repository.</param>
        /// <param name="nofticationRepository">The noftication repository.</param>
        public PostService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReplyCommentRepository replyCommentRepository,
            IFieldRepository fieldRepository,
            IFollowRepository followRepository,
            IUpVoteRepository upVoteRepository,
            IDownVoteRepository downVoteRepository,
            IClientGroupRepository clientGroupRepository,
            IFcmRepository fcmRepository,
            INofticationRepository nofticationRepository)
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
            this.clientGroupRepository = clientGroupRepository;
            this.fcmRepository = fcmRepository;
            this.nofticationRepository = nofticationRepository;
        }



        /// <summary>
        /// Adds the media.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Post đã bị xóa</exception>
        public async Task<AddMediaResponse> AddMedia(AddMediaRequest request)
        {
            Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                Image image = PostAdapter.FromRequest(request, httpContextAccessor);
                currentPost.MediaContents.Add(image);
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                return PostAdapter.ToResponse(image, currentPost.Id.ToString());
            }
            else throw new Exception("Post đã bị xóa");
        }

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            currentUser.PostCount++;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            Post post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();
            post.AuthorAvatar = currentUser.AvatarHash;

            post.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

            foreach (string fieldId in request.Fields)
            {
                Field field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
                if (field != null)
                    post.Fields.Add(field);
            }

            await postRepository.AddAsync(post);

            ClientGroup clientGroup = new ClientGroup()
            {
                Name = post.Id.ToString(),

            };
            clientGroup.UserIds.Add(post.AuthorId);
            await clientGroupRepository.AddAsync(clientGroup);

            return PostAdapter.ToResponse(post, currentUser.Id.ToString());
        }


        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy bài viết</exception>
        public async Task<GetPostByIdResponse> GetPostById(string postId)
        {
            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
            if (post == null || post.Status != ItemStatus.Active)
                throw new Exception("Không tìm thấy bài viết ");
            return PostAdapter.ToResponse(post);
        }

        /// <summary>
        /// Gets the post by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Người dùng chưa có bài viết nào</exception>
        public async Task<IEnumerable<Post>> GetPostByUserId(string userId, int skip, int count)
        {
            try
            {
                FilterDefinitionBuilder<Post> filter = Builders<Post>.Filter;
                FilterDefinition<Post> match = filter.Eq("author_id", userId) & filter.Eq("status", ItemStatus.Active);

                return (await postRepository.FindListAsync(match)).Skip(skip).Take(count);
            }
            catch (Exception)
            {
                throw new Exception("Người dùng chưa có bài viết nào");
            }


        }

        /// <summary>
        /// Gets the post timeline asynchronous.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetPostTimelineAsync(int skip, int count)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            FilterDefinition<Follow> findFilter = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);

            List<Follow> listFollow = await followRepository.FindListAsync(findFilter);

            List<string> listAuthor = new List<string>();
            listAuthor.Add(currentUser.Id.ToString());
            foreach (Follow item in listFollow)
                listAuthor.Add(item.ToId);

            List<Post> result = new List<Post>();

            foreach (string author in listAuthor)
            {
                FilterDefinitionBuilder<Post> builder = Builders<Post>.Filter;
                FilterDefinition<Post> postFindFilter = builder.Eq("author_id", author) & builder.Eq("status", ItemStatus.Active);
                result.AddRange(await postRepository.FindListAsync(postFindFilter));
            }
            return result.Skip(skip).Take(count).OrderByDescending(x => x.CreatedDate).ToList();
        }


        /// <summary>
        /// Synchronizes the comment.
        /// </summary>
        public async Task SyncComment()
        {
            try
            {
                List<Post> posts = postRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (Post post in posts)
                {
                    IQueryable<Comment> latestComments = commentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.PostId == post.Id.ToString() && x.Status == ItemStatus.Active);
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

        /// <summary>
        /// Synchronizes the reply.
        /// </summary>
        public async Task SyncReply()
        {
            try
            {
                List<Comment> comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (Comment comment in comments)
                {
                    IQueryable<ReplyComment> latestReplies = replyCommentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.ParentId == comment.Id.ToString() && x.Status == ItemStatus.Active);
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

        /// <summary>
        /// Upvotes the specified post identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Uncompleted activity</exception>
        public async Task<string> Upvote(string postId)
        {
            try
            {
                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

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

                FilterDefinition<ClientGroup> filter = Builders<ClientGroup>.Filter.Eq("name", currentPost.OId);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(filter);

                if (!clientGroup.UserIds.Contains(currentUser.OId))
                {
                    clientGroup.UserIds.Add(currentUser.OId);
                    await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
                }

                if (currentPost.AuthorId != currentUser.OId) //Cùng tác giả
                {
                    Noftication notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã upvote bài viết của {currentPost.AuthorName} ",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }
                else //Khác tác giả
                {
                    Noftication notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã upvote bài viết của bạn",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }

                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }
        /// <summary>
        /// Downvotes the specified post identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Uncompleted activity</exception>
        public async Task<string> Downvote(string postId)
        {

            try
            {
                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

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

                FilterDefinition<ClientGroup> filter = Builders<ClientGroup>.Filter.Eq("name", currentPost.OId);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(filter);

                if (!clientGroup.UserIds.Contains(currentUser.OId))
                {
                    clientGroup.UserIds.Add(currentUser.OId);
                    await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
                }

                if (currentPost.AuthorId != currentUser.OId) //Cùng tác giả
                {
                    Noftication notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã downvote bài viết của {currentPost.AuthorName} ",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }
                else //Khác tác giả
                {
                    Noftication notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã downvote bài viết của bạn",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }

                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }

        /// <summary>
        /// Updates the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<Post> UpdatePost(UpdatePostRequest request)
        {
            Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

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

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Bài viết không tồn tại hoặc đã bị xóa.</exception>
        public async Task<Post> SavePost(string id)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
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

        /// <summary>
        /// Gets the saved post.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public async Task<List<Post>> GetSavedPost(int skip, int count)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<Post> result = new List<Post>();
            foreach (string postId in currentUser.PostSaved)
            {
                Post post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
                if (post != null)
                    result.Add(post);
            }
            return result.Skip(skip).Take(count).ToList();
        }

        /// <summary>
        /// Filters the specified filter request.
        /// </summary>
        /// <param name="filterRequest">The filter request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> Filter(FilterRequest filterRequest)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            FilterDefinition<Follow> findFilter = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);

            List<Follow> listFollow = await followRepository.FindListAsync(findFilter);

            List<string> listAuthor = new List<string>();

            foreach (Follow item in listFollow)
                listAuthor.Add(item.ToId);
            listAuthor.Add(currentUser.OId);
            List<Post> timelines = new List<Post>();

            foreach (string author in listAuthor)
            {
                FilterDefinitionBuilder<Post> builder = Builders<Post>.Filter;
                FilterDefinition<Post> postFindFilter = builder.Eq("author_id", author) & builder.Eq("status", ItemStatus.Active);
                timelines.AddRange(await postRepository.FindListAsync(postFindFilter));
            }

            IQueryable<Post> queryable = timelines.AsQueryable();

            if (!String.IsNullOrEmpty(filterRequest.KeyWord))
                queryable = queryable.Where(x => x.Title.ToLower().Contains(filterRequest.KeyWord.ToLower()));
            if (filterRequest.FromDate != null)
                queryable = queryable.Where(x => x.CreatedDate >= filterRequest.FromDate);
            if (filterRequest.ToDate != null)
                queryable = queryable.Where(x => x.CreatedDate <= filterRequest.ToDate);
            if (!String.IsNullOrEmpty(filterRequest.Field))
            {
                Field field = fieldRepository.GetById(ObjectId.Parse(filterRequest.Field));
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


        /// <summary>
        /// Synchronizes the vote.
        /// </summary>
        public async Task SyncVote()
        {
            try
            {
                IQueryable<Comment> comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
                foreach (Comment comment in comments)
                {
                    FilterDefinition<UpVote> upvoteBuilder = Builders<UpVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.UpvoteCount = (await upVoteRepository.FindListAsync(upvoteBuilder)).Count;
                    await commentRepository.UpdateAsync(comment, comment.Id);

                    FilterDefinition<DownVote> downVoteBuilder = Builders<DownVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.DownvoteCount = (await downVoteRepository.FindListAsync(downVoteBuilder)).Count;
                    await commentRepository.UpdateAsync(comment, comment.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        /// <summary>
        /// Synchronizes the reply vote.
        /// </summary>
        public async Task SyncReplyVote()
        {
            try
            {
                IQueryable<ReplyComment> comments = replyCommentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
                foreach (ReplyComment comment in comments)
                {
                    FilterDefinition<UpVote> upvoteBuilder = Builders<UpVote>.Filter.Eq("object_vote_id", comment.OId);
                    comment.UpvoteCount = (await upVoteRepository.FindListAsync(upvoteBuilder)).Count;
                    await replyCommentRepository.UpdateAsync(comment, comment.Id);

                    FilterDefinition<DownVote> downVoteBuilder = Builders<DownVote>.Filter.Eq("object_vote_id", comment.OId);
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
