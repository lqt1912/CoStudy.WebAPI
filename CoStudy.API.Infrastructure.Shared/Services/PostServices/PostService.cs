using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.ViewModels;
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
        /// The mapper
        /// </summary>
        IMapper mapper;


        /// <summary>
        /// The object level repository
        /// </summary>
        IObjectLevelRepository objectLevelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService" /> class.
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
        /// <param name="mapper">The mapper.</param>
        /// <param name="objectLevelRepository">The object level repository.</param>
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
            INofticationRepository nofticationRepository,
            IMapper mapper, IObjectLevelRepository objectLevelRepository)
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
            this.mapper = mapper;
            this.objectLevelRepository = objectLevelRepository;
        }

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<PostViewModel> AddPost(AddPostRequest request)
        {

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);


            Post post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();


            await postRepository.AddAsync(post);

            await fcmRepository.AddToGroup(
                     new AddUserToGroupRequest()
                     {
                         GroupName = post.OId,
                         Type = Feature.GetTypeName(post),
                         UserIds = new List<string> { currentUser.OId }
                     }
                     );

            PostViewModel response = mapper.Map<PostViewModel>(post);
            return response;
        }

        /// <summary>
        /// Gets the post by user identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Người dùng chưa có bài viết nào</exception>
        public async Task<IEnumerable<PostViewModel>> GetPostByUserId(GetPostByUserRequest request)
        {
            try
            {
                FilterDefinitionBuilder<Post> filter = Builders<Post>.Filter;
                FilterDefinition<Post> match = filter.Eq("author_id", request.UserId) & filter.Eq("status", ItemStatus.Active);
                User currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);

                User author = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

                List<Post> data = await postRepository.FindListAsync(match);
                if (request.Skip.HasValue && request.Count.HasValue)
                {
                    data = data.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
                }

                IEnumerable<PostViewModel> response = mapper.Map<IEnumerable<PostViewModel>>(data);
                return response;
            }
            catch (Exception)
            {
                throw new Exception("Người dùng chưa có bài viết nào");
            }
        }

        /// <summary>
        /// Gets the post timeline asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PostViewModel>> GetPostTimelineAsync(BaseGetAllRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            FilterDefinition<Follow> findFilter = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);

            List<Follow> listFollow = await followRepository.FindListAsync(findFilter);

            List<string> listAuthor = new List<string>();
            listAuthor.Add(currentUser.Id.ToString());
            foreach (Follow item in listFollow)
            {
                listAuthor.Add(item.ToId);
            }

            List<Post> result = new List<Post>();

            foreach (string author in listAuthor)
            {
                FilterDefinitionBuilder<Post> builder = Builders<Post>.Filter;
                FilterDefinition<Post> postFindFilter = builder.Eq("author_id", author) & builder.Eq("status", ItemStatus.Active);
                result.AddRange(await postRepository.FindListAsync(postFindFilter));
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value).OrderByDescending(x => x.CreatedDate).ToList();
            }

            IEnumerable<PostViewModel> data = mapper.Map<IEnumerable<PostViewModel>>(result);

            return data;
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
                User currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);
                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                FilterDefinitionBuilder<UpVote> builderUpvote = Builders<UpVote>.Filter;
                FilterDefinition<UpVote> filterExistUpvote = builderUpvote.Eq("object_vote_id", postId)
                    & builderUpvote.Eq("upvote_by", currentuser.OId)
                    & builderUpvote.Eq("is_deleted", false);

                UpVote existUpvote = await upVoteRepository.FindAsync(filterExistUpvote);
                if (existUpvote != null)
                {
                    return "Bạn đã Upvote bài viết rồi";
                }
                else if (existUpvote == null)
                {
                    FilterDefinitionBuilder<DownVote> builderDownVote = Builders<DownVote>.Filter;
                    FilterDefinition<DownVote> filterExistDownVote = builderDownVote.Eq("object_vote_id", postId)
                        & builderDownVote.Eq("downvote_by", currentuser.OId)
                        & builderDownVote.Eq("is_deleted", false);
                    DownVote existDownVote = await downVoteRepository.FindAsync(filterExistDownVote);

                    if (existDownVote != null)
                    {
                        existDownVote.IsDeleted = true;
                        await downVoteRepository.DeleteAsync(existDownVote.Id);
                    }

                    UpVote upvote = new UpVote()
                    {
                        ObjectVoteId = postId,
                        UpVoteBy = currentuser.OId
                    };
                    await upVoteRepository.AddAsync(upvote);

                }


                var notification = new Noftication()
                {
                    AuthorId = currentuser.OId,
                    OwnerId = currentPost.AuthorId,
                    ContentType = ContentType.UPVOTE_POST_NOTIFY,
                    ObjectId = currentPost.OId
                };

               await  fcmRepository.PushNotify(currentPost.OId, notification);

                return "Upvote thành công. ";
            }
            catch (Exception)
            {
                return "Có lỗi xảy ra. ";
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

                User currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);
                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                FilterDefinitionBuilder<DownVote> builderDownVote = Builders<DownVote>.Filter;
                FilterDefinition<DownVote> filterExistDownvote = builderDownVote.Eq("object_vote_id", postId)
                    & builderDownVote.Eq("downvote_by", currentuser.OId)
                    & builderDownVote.Eq("is_deleted", false);

                DownVote existDownVote = await downVoteRepository.FindAsync(filterExistDownvote);
                if (existDownVote != null)
                {
                    return "Bạn đã Down bài viết rồi";
                }
                else if (existDownVote == null)
                {
                    FilterDefinitionBuilder<UpVote> builderUpVote = Builders<UpVote>.Filter;
                    FilterDefinition<UpVote> filterExistUpVote = builderUpVote.Eq("object_vote_id", postId)
                        & builderUpVote.Eq("upvote_by", currentuser.OId)
                        & builderUpVote.Eq("is_deleted", false);

                    UpVote existUpVote = await upVoteRepository.FindAsync(filterExistUpVote);

                    if (existUpVote != null)
                    {
                        existUpVote.IsDeleted = true;
                        await upVoteRepository.DeleteAsync(existUpVote.Id);
                    }

                    DownVote downvote = new DownVote()
                    {
                        ObjectVoteId = postId,
                        DownVoteBy = currentuser.OId
                    };
                    await downVoteRepository.AddAsync(downvote);

                }


               

                var notification = new Noftication()
                {
                    AuthorId = currentuser.OId,
                    OwnerId = currentPost.AuthorId,
                    ContentType = ContentType.DOWNVOTE_POST_NOTIFY,
                    ObjectId = currentPost.OId
                };

                await fcmRepository.PushNotify(currentPost.OId, notification);

                return "Downvote thành công. ";
            }
            catch (Exception)
            {
                return "Có lỗi xảy ra. ";
            }
        }

        /// <summary>
        /// Updates the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Có lỗi xảy ra khi tìm kiếm bài viết.</exception>
        public async Task<PostViewModel> UpdatePost(UpdatePostRequest request)
        {
            Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            if (currentPost != null)
            {
                currentPost.StringContents = request.StringContents;
                currentPost.MediaContents = request.MediaContents;
                currentPost.Title = request.Title;
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                PostViewModel response = mapper.Map<PostViewModel>(currentPost);
                return response;
            }
            throw new Exception("Có lỗi xảy ra khi tìm kiếm bài viết. ");
        }

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Bài viết không tồn tại hoặc đã bị xóa.</exception>
        public async Task<PostViewModel> SavePost(string id)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            if (post != null)
            {
                if (!currentUser.PostSaved.Contains(id))
                {
                    currentUser.PostSaved.Add(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                }
                else
                {
                    currentUser.PostSaved.Remove(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                }

                PostViewModel response = mapper.Map<PostViewModel>(post);
                return response;
            }
            else
            {
                throw new Exception("Bài viết không tồn tại hoặc đã bị xóa. ");
            }
        }

        /// <summary>
        /// Gets the saved post.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<PostViewModel>> GetSavedPost(BaseGetAllRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<Post> result = new List<Post>();
            foreach (string postId in currentUser.PostSaved)
            {
                Post post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
                if (post != null)
                {
                    result.Add(post);
                }
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
            }

            return mapper.Map<List<PostViewModel>>(result);
        }

        /// <summary>
        /// Filters the specified filter request.
        /// </summary>
        /// <param name="filterRequest">The filter request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PostViewModel>> Filter(FilterRequest filterRequest)
        {
            var posts = postRepository.GetAll();

            var builders = Builders<Post>.Filter;
            FilterDefinition<Post> filterParam = builders.Eq("status", ItemStatus.Active);

            if (filterRequest.ContentFilter != null)
            {
                filterParam = filterParam & (builders.Regex("string_contents.content", filterRequest.ContentFilter.KeyWord)
                    | builders.Regex("title", filterRequest.ContentFilter.KeyWord));
            }

            if (filterRequest.CreatedDateFilter != null)
            {
                if (filterRequest.CreatedDateFilter.FromDate.HasValue && filterRequest.CreatedDateFilter.ToDate.HasValue)
                    filterParam = filterParam
                        & builders.Gt("created_date", filterRequest.CreatedDateFilter.FromDate)
                        & builders.Lt("created_date", filterRequest.CreatedDateFilter.ToDate);
            }

            posts = (await postRepository.FindListAsync(filterParam)).AsQueryable();


            if (filterRequest.CreatedDateFilter != null)
            {



                if (filterRequest.CreatedDateFilter.IsSortDescending == true)
                    posts = posts.OrderByDescending(x => x.CreatedDate);
                else
                    posts = posts.OrderBy(x => x.CreatedDate);
            }

            if (filterRequest.ContentFilter != null)
            {

                if (filterRequest.ContentFilter.IsSortDescending == true)
                    posts = posts.OrderByDescending(x => x.Title);
                else
                    posts = posts.OrderBy(x => x.Title);

            }

            var vm = mapper.Map<IEnumerable<PostViewModel>>(posts);

            if (filterRequest.UpvoteCountFilter != null)
            {
                if (filterRequest.UpvoteCountFilter.ValueFrom.HasValue && filterRequest.UpvoteCountFilter.ValueTo.HasValue)
                {
                    vm = vm.Where(x => x.Upvote >= filterRequest.UpvoteCountFilter.ValueFrom.Value && x.Upvote <= filterRequest.UpvoteCountFilter.ValueTo.Value);
                }

                if (filterRequest.UpvoteCountFilter.IsSortDescending == true)
                    vm = vm.OrderByDescending(x => x.Upvote);
                else
                    vm = vm.OrderBy(x => x.Upvote);
            }

            if (filterRequest.CommentCountFilter != null)
            {
                if (filterRequest.CommentCountFilter.ValueFrom.HasValue && filterRequest.CommentCountFilter.ValueTo.HasValue)
                {

                    vm = vm.Where(x => x.CommentCount >= filterRequest.CommentCountFilter.ValueFrom.Value
                    && x.CommentCount <= filterRequest.CommentCountFilter.ValueTo.Value);
                }

                if (filterRequest.CommentCountFilter.IsSortDescending == true)
                    vm = vm.OrderByDescending(x => x.CommentCount);
                else
                    vm = vm.OrderBy(x => x.CommentCount);
            }

            var result = new List<PostViewModel>();
            foreach (var post in vm)
            {
                if (IsMatch(post, filterRequest.LevelFilter) == true)
                    result.Add(post);
            }

            return result;
        }


        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy post</exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostViewModel> GetPostById1(string id)
        {
            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            if (post != null)
            {
                return mapper.Map<PostViewModel>(post);
            }
            throw new Exception("Không tìm thấy post");
        }



        /// <summary>
        /// Determines whether the specified post is match.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="objectLevel">The object level.</param>
        /// <returns>
        ///   <c>true</c> if the specified post is match; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMatch(PostViewModel post, ObjectLevel objectLevel)
        {
            var objlvls = objectLevelRepository.GetAll().Where(x => x.ObjectId == post.OId);

            if (string.IsNullOrEmpty(objectLevel.FieldId) || string.IsNullOrEmpty(objectLevel.LevelId))
                return true;

            foreach (var item in objlvls)
            {
                if (item.FieldId == objectLevel.FieldId)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
