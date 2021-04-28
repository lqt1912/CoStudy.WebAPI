using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
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
        /// The FCM repository
        /// </summary>
        IFcmRepository fcmRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;


        /// <summary>
        /// The object level repository
        /// </summary>
        IObjectLevelRepository objectLevelRepository;

        /// <summary>
        /// The notification object repository
        /// </summary>
        INotificationObjectRepository notificationObjectRepository;

        /// <summary>
        /// The level service
        /// </summary>
        ILevelService levelService;

        /// <summary>
        /// The field group repository
        /// </summary>
        IFieldGroupRepository fieldGroupRepository;

        /// <summary>
        /// The levelRepository
        /// </summary>
        ILevelRepository levelRepository;

        IMessageService messageService;

        IConversationService conversationService;
        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="followRepository">The follow repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="fcmRepository">The FCM repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="objectLevelRepository">The object level repository.</param>
        /// <param name="notificationObjectRepository">The notification object repository.</param>
        /// <param name="levelService">The level service.</param>
        public PostService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IUserRepository userRepository,
            IPostRepository postRepository,
            IFollowRepository followRepository,
            IUpVoteRepository upVoteRepository,
            IDownVoteRepository downVoteRepository,
            IFcmRepository fcmRepository,
            IMapper mapper, IObjectLevelRepository objectLevelRepository,
            INotificationObjectRepository notificationObjectRepository,
            ILevelService levelService,
            IFieldGroupRepository fieldGroupRepository,
            ILevelRepository levelRepository,
            IMessageService messageService, IConversationService conversationService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.followRepository = followRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.fcmRepository = fcmRepository;
            this.mapper = mapper;
            this.objectLevelRepository = objectLevelRepository;
            this.notificationObjectRepository = notificationObjectRepository;
            this.levelService = levelService;
            this.fieldGroupRepository = fieldGroupRepository;
            this.levelRepository = levelRepository;
            this.messageService = messageService;
            this.conversationService = conversationService;
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

            ///Check allowed word
            foreach (var stringContent in post.StringContents)
            {
                if (StringUtils.ValidateAllowString(configuration, stringContent.Content) == false)
                    throw new Exception("Nội dung có chứa từ ngữ không hợp lệ. ");
            }

            if (StringUtils.ValidateAllowString(configuration, post.Title) == false)
                throw new Exception("Tiêu đề có chứa từ ngữ không hợp lệ. ");

            await postRepository.AddAsync(post);

            await fcmRepository.AddToGroup(
                     new AddUserToGroupRequest()
                     {
                         GroupName = post.OId,
                         Type = Feature.GetTypeName(post),
                         UserIds = new List<string> { currentUser.OId }
                     }
                     );

            foreach (var item in request.Fields)
                item.ObjectId = post.OId;

            await levelService.AddObjectLevel(request.Fields);

            var userMatchs = await GetUsersMatchPostField(post);

            var notificationObjectBuilders = Builders<NotificationObject>.Filter;

            var notificationObjectFilters = notificationObjectBuilders.Eq("object_id", post.OId)
                & notificationObjectBuilders.Eq("notification_type", "ADD_POST_NOTIFY");

            var existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

            string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

            if (existNotificationObject == null)
            {
                var newNotificationObject = new NotificationObject()
                {
                    NotificationType = "ADD_POST_NOTIFY",
                    ObjectId = post.OId,
                    OwnerId = post.AuthorId
                };
                await notificationObjectRepository.AddAsync(newNotificationObject);
                notificationObject = newNotificationObject.OId;
            }

            var notificationDetail = new NotificationDetail()
            {
                CreatorId = currentUser.OId,
                NotificationObjectId = notificationObject
            };

            foreach (var user in userMatchs)
            {
                await fcmRepository.PushNotifyPostMatch(user.OId, notificationDetail);
            }

          

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

                var notificationObjectBuilders = Builders<NotificationObject>.Filter;

                var notificationObjectFilters = notificationObjectBuilders.Eq("object_id", postId)
                    & notificationObjectBuilders.Eq("notification_type", "UPVOTE_POST_NOTIFY");

                var existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                if (existNotificationObject == null)
                {
                    var newNotificationObject = new NotificationObject()
                    {
                        NotificationType = "UPVOTE_POST_NOTIFY",
                        ObjectId = postId,
                        OwnerId = currentPost.AuthorId
                    };
                    await notificationObjectRepository.AddAsync(newNotificationObject);
                    notificationObject = newNotificationObject.OId;
                }

                var notificationDetail = new NotificationDetail()
                {
                    CreatorId = currentuser.OId,
                    NotificationObjectId = notificationObject
                };


                await fcmRepository.PushNotifyDetail(currentPost.OId, notificationDetail);

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


                var notificationObjectBuilders = Builders<NotificationObject>.Filter;

                var notificationObjectFilters = notificationObjectBuilders.Eq("object_id", postId)
                    & notificationObjectBuilders.Eq("notification_type", "DOWNVOTE_POST_NOTIFY");

                var existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                if (existNotificationObject == null)
                {
                    var newNotificationObject = new NotificationObject()
                    {
                        NotificationType = "DOWNVOTE_POST_NOTIFY",
                        ObjectId = postId,
                        OwnerId = currentPost.AuthorId
                    };
                    await notificationObjectRepository.AddAsync(newNotificationObject);
                    notificationObject = newNotificationObject.OId;
                }

                var notificationDetail = new NotificationDetail()
                {
                    CreatorId = currentuser.OId,
                    NotificationObjectId = notificationObject
                };

                await fcmRepository.PushNotifyDetail(currentPost.OId, notificationDetail);

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

                request.Fields.PostId = currentPost.OId;

                await levelService.UpdatePostField(request.Fields);

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
                filterParam = filterParam & (builders.Regex("string_contents.content", filterRequest.ContentFilter)
                    | builders.Regex("title", filterRequest.ContentFilter));
            }

            if (filterRequest.FromDate != null && filterRequest.ToDate != null)
            {
                filterParam = filterParam
                    & builders.Gt("created_date", filterRequest.FromDate)
                    & builders.Lt("created_date", filterRequest.ToDate);
            }

            posts = (await postRepository.FindListAsync(filterParam)).AsQueryable();

            var vm = mapper.Map<IEnumerable<PostViewModel>>(posts);

            if (filterRequest.SortObject.HasValue && filterRequest.SortType.HasValue)
            {
                switch (filterRequest.SortObject.Value)
                {
                    case SortObject.Upvote:
                        {
                            var sortType = filterRequest.SortType.Value;
                            if (sortType == SortType.Ascending)
                                vm = vm.OrderBy(x => x.Upvote);
                            else if (sortType == SortType.Descending)
                                vm = vm.OrderByDescending(x => x.Upvote);
                            break;
                        }
                    case SortObject.Comment:
                        {
                            var sortType = filterRequest.SortType.Value;
                            if (sortType == SortType.Ascending)
                                vm = vm.OrderBy(x => x.CommentCount);
                            else if (sortType == SortType.Descending)
                                vm = vm.OrderByDescending(x => x.CommentCount);
                            break;

                        }
                    case SortObject.CreatedDate:
                        {
                            var sortType = filterRequest.SortType.Value;
                            if (sortType == SortType.Ascending)
                                vm = vm.OrderBy(x => x.CreatedDate);
                            else if (sortType == SortType.Descending)
                                vm = vm.OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                }
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
        /// Gets the news feed.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PostViewModel>> GetNewsFeed(NewsFeedRequest request)
        {
            var dataSource = new List<Post>();

            //Lấy danh sách post theo ngày
            if (request.FromDate.HasValue && request.ToDate.HasValue)
            {
                var fromDate = request.FromDate.Value;
                var toDaTe = request.ToDate.Value;
                var postBuilder = Builders<Post>.Filter;
                var postFilter = postBuilder.Lt("created_date", toDaTe)
                    & postBuilder.Gt("created_date", fromDate);
                var posts = await postRepository.FindListAsync(postFilter);
                // dataSource = posts;
            }

            //Lấy danh sách user_id đang theo dõi
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var userBuilder = Builders<Follow>.Filter.Eq("from_id", currentUser.OId);
            var follows = await followRepository.FindListAsync(userBuilder);
            var followIds = follows.Select(x => x.ToId).Distinct();

            //Lấy danh sách post của chính mình 
            var postByCurrentBuilder = Builders<Post>.Filter;
            var posytByCurrentFilter = postByCurrentBuilder.Eq("author_id", currentUser.OId);
            var postByCurrent = await postRepository.FindListAsync(posytByCurrentFilter);
            dataSource.AddRange(postByCurrent);


            //Duyệt danh sách và lấy ra những post của người dùng đang theo dõi. 
            foreach (var followId in followIds)
            {
                var postByAuthorBuilder = Builders<Post>.Filter;
                var postByAuthorFilter = postByAuthorBuilder.Eq("author_id", followId)
                    & postByAuthorBuilder.Eq("status", ItemStatus.Active);
                var postByAuthor = await postRepository.FindListAsync(postByAuthorFilter);
                dataSource.AddRange(postByAuthor);
            }

            //Lấy danh sách field là những thế mạnh của người dùng
            var userForcesBuilder = Builders<ObjectLevel>.Filter;
            var userForcesFilter = userForcesBuilder.Eq("object_id", currentUser.OId);
            var userForcesByObjectLevel = await objectLevelRepository.FindListAsync(userForcesFilter);
            var userForces = userForcesByObjectLevel.Select(x => x.FieldId).Distinct();

            //Lấy danh sách field_group từ danh sách field bên trên
            var fieldGroups = fieldGroupRepository.GetAll();
            var fieldGroupsFinal = new List<FieldGroup>();
            foreach (var fieldId in userForces)
            {
                foreach (var fieldGroup in fieldGroups)
                {
                    if (fieldGroup.FieldId.Contains(fieldId))
                        fieldGroupsFinal.Add(fieldGroup);
                }
            }
            fieldGroupsFinal = fieldGroupsFinal.Distinct().ToList();

            //Duyệt danh sách field_group để lấy các field liên quan trong group.
            var fieldToFilter = new List<string>();
            fieldGroupsFinal.ForEach(x => fieldToFilter.AddRange(x.FieldId));
            fieldToFilter = fieldToFilter.Distinct().ToList();

            //Từ danh sách field mở rộng này, lấy dang sách các object thuộc về field đó
            var listObjectId = new List<string>();
            foreach (var x in fieldToFilter)
            {
                var objectLevelBuilder = Builders<ObjectLevel>.Filter;
                var objectLevelFilter = objectLevelBuilder.Eq("field_id", x)
                    & objectLevelBuilder.Eq("is_active", true);
                var objectLevels = await objectLevelRepository.FindListAsync(objectLevelFilter);
                listObjectId.AddRange(objectLevels.Select(x => x.ObjectId));
            }
            listObjectId = listObjectId.Distinct().ToList();

            //Lấy danh sách các post có id trong danh sách trên
            var _posts = new List<Post>();
            listObjectId.ForEach(async x =>
            {
                var post = await postRepository.GetByIdAsync(ObjectId.Parse(x));
                if (post != null)
                    _posts.Add(post);
            });
            var _postIds = _posts.Select(x => x.OId);

            //Kết hợp với datasoure ban đầu9
            //dataSource = dataSource.Where(x => _postIds.Contains(x.OId)).Distinct().ToList();
            dataSource.AddRange(_posts);
            dataSource = dataSource.GroupBy(x => x.OId).Select(grp => grp.FirstOrDefault()).ToList();

            if (request.Skip.HasValue && request.Count.HasValue)
                dataSource = dataSource.Skip(request.Skip.Value).Take(request.Count.Value).ToList();

            return mapper.Map<List<PostViewModel>>(dataSource);
        }

        /// <summary>
        /// Shares the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<MessageViewModel>> SharePost(SharePostRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var addConversationRequest = new AddConversationRequest();

            addConversationRequest.Participants.AddRange(
                new List<ConversationMember>() {
                new ConversationMember() {  MemberId = request.UserId}
                });

            var conversation = await conversationService.AddConversation(addConversationRequest);

            var addMessageRequest = new AddMessageRequest()
            {
                PostId = request.PostId,
                MessageType = MessageBaseType.PostThumbnail,
                ConversationId = conversation.OId
            };
            await messageService.AddMessage(addMessageRequest);

            var messageByConversationRequest = new GetMessageByConversationIdRequest()
            {
                ConversationId = conversation.OId,
                Skip = request.Skip,
                Count = request.Count
            };

            var result = await messageService.GetMessageByConversationId(messageByConversationRequest);
            return result;
        }


        /// <summary>
        /// Gets the users match post field.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <returns></returns>
        private async Task<List<User>> GetUsersMatchPostField(Post post)
        {
            var postObjectLevelBuilder = Builders<ObjectLevel>.Filter;
            var postObjectLevelFilter = postObjectLevelBuilder.Eq("object_id", post.OId)
                & postObjectLevelBuilder.Eq("is_active", true);
            var postObjectLevels = await objectLevelRepository.FindListAsync(postObjectLevelFilter);
            var allUsers = userRepository.GetAll();

            var result = new List<User>();
            foreach (var user in allUsers)
            {
                if (true == (await IsUserMatchListObjectLevel(user, postObjectLevels)))
                    result.Add(user);
            }
            return result;

        }

        /// <summary>
        /// Determines whether [is user match list object level] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns>
        ///   <c>true</c> if [is user match list object level] [the specified user]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> IsUserMatchListObjectLevel(User user, List<ObjectLevel> objectLevels)
        {
            var userObjectLevelBuilder = Builders<ObjectLevel>.Filter;
            var userObjectLevelFilter = userObjectLevelBuilder.Eq("object_id", user.OId)
                & userObjectLevelBuilder.Eq("is_active", true);
            var userObjectLevels = await objectLevelRepository.FindListAsync(userObjectLevelFilter);
            foreach (var userObjectLevel in userObjectLevels)
            {
                var _ = objectLevels.FirstOrDefault(x => x.FieldId == userObjectLevel.FieldId && x.LevelId == userObjectLevel.LevelId);
                if (_ != null)
                    return true;
            }
            return false;
        }

        private bool IsMatch(PostViewModel post, LevelFilterItem levelFilterItem)
        {
            if (string.IsNullOrEmpty(levelFilterItem.FieldId) || string.IsNullOrEmpty(levelFilterItem.LevelId))
                return true;
            var objlvls = objectLevelRepository.GetAll().Where(x => x.ObjectId == post.OId);

            foreach (var item in objlvls)
            {
                Level lvlFilter = levelRepository.GetAll().FirstOrDefault(x => x.OId == levelFilterItem.LevelId);
                Level lvlPost = levelRepository.GetAll().FirstOrDefault(x => x.OId == item.LevelId);

                if (item.FieldId == levelFilterItem.FieldId && lvlFilter.Order <= lvlPost.Order)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsMatch(PostViewModel post, LevelFilter levelFilter)
        {
            foreach (var item in levelFilter.FilterItems)
            {
                if (IsMatch(post, item) == true)
                    return true;
            }
            return false;
        }
    }
}
