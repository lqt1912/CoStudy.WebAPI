using AutoMapper;
using Common;
using Common.Constant;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request;
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
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using static Common.Constant.FollowConstant;
using static Common.Constant.NotificationConstant;
using static Common.Constant.PostConstant;
using static Common.Constant.VoteConstant;
using static Common.Constants;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;
        private readonly IFollowRepository followRepository;
        private readonly IUpVoteRepository upVoteRepository;
        private readonly IDownVoteRepository downVoteRepository;
        private readonly IFcmRepository fcmRepository;
        private readonly IMapper mapper;
        private readonly IObjectLevelRepository objectLevelRepository;
        private readonly INotificationObjectRepository notificationObjectRepository;
        private readonly ILevelService levelService;
        private readonly IFieldGroupRepository fieldGroupRepository;
        private readonly ILevelRepository levelRepository;
        private readonly IMessageService messageService;
        private readonly IConversationService conversationService;

        public PostService(IHttpContextAccessor httpContextAccessor,
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

        public async Task<PostViewModel> AddPost(AddPostRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();

            try
            {
                foreach (var stringContent in post.StringContents)
                    if (StringUtils.ValidateAllowString(configuration, stringContent.Content) == false)
                        throw new Exception(UnAllowContent);
                if (StringUtils.ValidateAllowString(configuration, post.Title) == false)
                    throw new Exception(UnAllowTitle);
            }
            catch(Exception)
            {
                //do nothing
            }
            await postRepository.AddAsync(post);

            await fcmRepository.AddToGroup(new AddUserToGroupRequest()
            {
                GroupName = post.OId,
                Type = Feature.GetTypeName(post),
                UserIds = new List<string> { currentUser.OId }
            });

            foreach (var item in request.Fields)
                item.ObjectId = post.OId;

            await levelService.AddObjectLevel(request.Fields);

            var userMatchs = await GetUsersMatchPostField(post);

            foreach (var user in userMatchs)
            {
                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = currentUser.OId,
                    ObjectId = post.OId,
                    ObjectThumbnail = post.Title
                };

                await fcmRepository.PushNotify(user.OId, notificationDetail, NotificationContent.AddPostNotification);
            }
            var response = mapper.Map<PostViewModel>(post);
            return response;
        }

        public async Task<IEnumerable<PostViewModel>> GetPostByUserId(GetPostByUserRequest request)
        {
            try
            {
                var filter = Builders<Post>.Filter;
                var match = filter.Eq(PostConstant.AuthorId, request.UserId) & filter.Eq(Constants.Status, ItemStatus.Active);
                var currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);

                var author = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

                var data = await postRepository.FindListAsync(match);
                if (request.Skip.HasValue && request.Count.HasValue)
                {
                    data = data.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
                }

                var response = mapper.Map<IEnumerable<PostViewModel>>(data);
                return response;
            }
            catch (Exception)
            {
                throw new Exception(UserHasNoPost);
            }
        }

        public async Task<IEnumerable<PostViewModel>> GetPostTimelineAsync(BaseGetAllRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var findFilter = Builders<Follow>.Filter.Eq(FromId, currentUser.OId);

            var listFollow = await followRepository.FindListAsync(findFilter);

            var listAuthor = new List<string>();
            listAuthor.Add(currentUser.Id.ToString());
            foreach (var item in listFollow)
            {
                listAuthor.Add(item.ToId);
            }

            var result = new List<Post>();

            foreach (var author in listAuthor)
            {
                var builder = Builders<Post>.Filter;
                var postFindFilter = builder.Eq(AuthorId, author) & builder.Eq(Status, ItemStatus.Active);
                result.AddRange(await postRepository.FindListAsync(postFindFilter));
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value).OrderByDescending(x => x.CreatedDate).ToList();
            }

            var data = mapper.Map<IEnumerable<PostViewModel>>(result);

            return data;
        }

        public async Task<string> Upvote(string postId)
        {
            try
            {
                var currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                var builderUpvote = Builders<UpVote>.Filter;
                var filterExistUpvote = builderUpvote.Eq(ObjectVoteId, postId)
                                        & builderUpvote.Eq(UpVoteBy, currentuser.OId)
                                        & builderUpvote.Eq(IsDeleted, false);

                var existUpvote = await upVoteRepository.FindAsync(filterExistUpvote);
                if (existUpvote != null)
                {
                    return UserUpvoteAlready;
                }
                else if (existUpvote == null)
                {
                    var builderDownVote = Builders<DownVote>.Filter;
                    var filterExistDownVote = builderDownVote.Eq(ObjectVoteId, postId)
                                              & builderDownVote.Eq(DownVoteBy, currentuser.OId)
                                              & builderDownVote.Eq(IsDeleted, false);
                    var existDownVote = await downVoteRepository.FindAsync(filterExistDownVote);

                    if (existDownVote != null)
                    {
                        existDownVote.IsDeleted = true;
                        await downVoteRepository.DeleteAsync(existDownVote.Id);
                    }

                    var upvote = new UpVote()
                    {
                        ObjectVoteId = postId,
                        UpVoteBy = currentuser.OId
                    };
                    await upVoteRepository.AddAsync(upvote);
                }

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentuser.OId,
                    OwnerId = currentPost.AuthorId,
                    ObjectId = currentPost.OId,
                    ObjectThumbnail = currentPost.Title
                };

                await fcmRepository.PushNotify(currentPost.AuthorId, notificationDetail, NotificationContent.UpvotePostNotification);

                return UpvoteSuccess;
            }
            catch (Exception)
            {
                return "Có lỗi xảy ra. ";
            }
        }

        public async Task<string> Downvote(string postId)
        {
            try
            {
                var currentuser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                var builderDownVote = Builders<DownVote>.Filter;

                var filterExistDownvote = builderDownVote.Eq(ObjectVoteId, postId)
                                          & builderDownVote.Eq(DownVoteBy, currentuser.OId)
                                          & builderDownVote.Eq(IsDeleted, false);

                var existDownVote = await downVoteRepository.FindAsync(filterExistDownvote);

                if (existDownVote != null)
                {
                    return UserDownvoteAlready;
                }

                var builderUpVote = Builders<UpVote>.Filter;

                var filterExistUpVote = builderUpVote.Eq(ObjectVoteId, postId)
                                        & builderUpVote.Eq(UpVoteBy, currentuser.OId)
                                        & builderUpVote.Eq(IsDeleted, false);

                var existUpVote = await upVoteRepository.FindAsync(filterExistUpVote);

                if (existUpVote != null)
                {
                    existUpVote.IsDeleted = true;
                    await upVoteRepository.DeleteAsync(existUpVote.Id);
                }

                var downvote = new DownVote()
                {
                    ObjectVoteId = postId,
                    DownVoteBy = currentuser.OId
                };

                await downVoteRepository.AddAsync(downvote);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentuser.OId,
                    OwnerId = currentPost.AuthorId,
                    ObjectId = currentPost.OId,
                    ObjectThumbnail = currentPost.Title
                };

                await fcmRepository.PushNotify(currentPost.AuthorId, notificationDetail, NotificationContent.DownvotePostNotification);


                return DownvoteSuccess;
            }
            catch (Exception)
            {
                return "Có lỗi xảy ra. ";
            }
        }

        public async Task<PostViewModel> UpdatePost(UpdatePostRequest request)
        {
            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            if (currentPost != null && currentPost.Status== ItemStatus.Active)
            {
                currentPost.StringContents = request.StringContents;
                currentPost.MediaContents = request.MediaContents;
                currentPost.Title = request.Title;
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                request.Fields.PostId = currentPost.OId;

                await levelService.UpdatePostField(request.Fields);

                var response = mapper.Map<PostViewModel>(currentPost);

                return response;
            }
            throw new Exception(ErrorSearchPost);
        }

        public async Task<SavePostResponse> SavePost(string id)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            var result = new SavePostResponse()
            {
                PostId = post.OId
            };

            if (post != null)
            {
                if (!currentUser.PostSaved.Contains(id))
                {
                    currentUser.PostSaved.Add(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    result.IsSave = true;
                }
                else
                {
                    currentUser.PostSaved.Remove(id);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    result.IsSave = false;
                }
                return result;
            }
            else
            {
                throw new Exception(ErrorPostNotFound);
            }
        }

        public async Task<List<PostViewModel>> GetSavedPost(BaseGetAllRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var result = new List<Post>();
            foreach (var postId in currentUser.PostSaved)
            {
                var post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
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

        public async Task<IEnumerable<PostViewModel>> Filter(FilterRequest filterRequest)
        {
            var posts = postRepository.GetAll();

            var builders = Builders<Post>.Filter;
            var filterParam = builders.Eq(Status, ItemStatus.Active);

            if (filterRequest.ContentFilter != null)
            {
                filterParam = filterParam & (builders.Regex(PostStringContent, filterRequest.ContentFilter)
                    | builders.Regex(Title, filterRequest.ContentFilter));
            }

            if (filterRequest.FromDate != null && filterRequest.ToDate != null)
            {
                filterParam = filterParam
                    & builders.Gt(CreatedDate, filterRequest.FromDate)
                    & builders.Lt(CreatedDate, filterRequest.ToDate);
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
                            vm = sortType switch
                            {
                                SortType.Ascending => vm.OrderBy(x => x.Upvote),
                                SortType.Descending => vm.OrderByDescending(x => x.Upvote),
                                _ => vm
                            };
                            break;
                        }
                    case SortObject.Comment:
                        {
                            var sortType = filterRequest.SortType.Value;
                            vm = sortType switch
                            {
                                SortType.Ascending => vm.OrderBy(x => x.CommentCount),
                                SortType.Descending => vm.OrderByDescending(x => x.CommentCount),
                                _ => vm
                            };

                            break;

                        }
                    case SortObject.CreatedDate:
                        {
                            var sortType = filterRequest.SortType.Value;
                            vm = sortType switch
                            {
                                SortType.Ascending => vm.OrderBy(x => x.CreatedDate),
                                SortType.Descending => vm.OrderByDescending(x => x.CreatedDate),
                                _ => vm
                            };

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (filterRequest.LevelFilter != null && filterRequest.LevelFilter.FilterItems.Any())
            {
                var result = new List<PostViewModel>();
                foreach (var post in vm)
                {
                    if (IsMatch(post, filterRequest.LevelFilter) == true)
                    {
                        result.Add(post);
                    }
                }
                return result;
            }
            return mapper.Map<List<PostViewModel>>(posts.ToList());

        }

        public async Task<PostViewModel> GetPostById1(string id)
        {
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            if (post != null && post.Status == ItemStatus.Active)
            {
                return mapper.Map<PostViewModel>(post);
            }
            throw new Exception("Không tìm thấy post");
        }

        public async Task<IEnumerable<PostViewModel>> GetNewsFeed(NewsFeedRequest request)
        {
            var dataSource = new List<Post>();

            //Lấy danh sách post theo ngày
            if (request.FromDate.HasValue && request.ToDate.HasValue)
            {
                var fromDate = request.FromDate.Value;
                var toDaTe = request.ToDate.Value;
                var postBuilder = Builders<Post>.Filter;
                var postFilter = postBuilder.Lt(CreatedDate, toDaTe)
                    & postBuilder.Gt(CreatedDate, fromDate);
                var posts = await postRepository.FindListAsync(postFilter);
                // dataSource = posts;
            }

            //Lấy danh sách user_id đang theo dõi
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var userBuilder = Builders<Follow>.Filter.Eq(FromId, currentUser.OId);
            var follows = await followRepository.FindListAsync(userBuilder);
            var followIds = follows.Select(x => x.ToId).Distinct();

            //Lấy danh sách post của chính mình 
            var postByCurrentBuilder = Builders<Post>.Filter;
            var posytByCurrentFilter = postByCurrentBuilder.Eq(AuthorId, currentUser.OId);
            var postByCurrent = await postRepository.FindListAsync(posytByCurrentFilter);
            dataSource.AddRange(postByCurrent);


            //Duyệt danh sách và lấy ra những post của người dùng đang theo dõi. 
            foreach (var followId in followIds)
            {
                var postByAuthorBuilder = Builders<Post>.Filter;
                var postByAuthorFilter = postByAuthorBuilder.Eq(AuthorId, followId)
                    & postByAuthorBuilder.Eq(Status, ItemStatus.Active);
                var postByAuthor = await postRepository.FindListAsync(postByAuthorFilter);
                dataSource.AddRange(postByAuthor);
            }

            //Lấy danh sách field là những thế mạnh của người dùng
            var userForcesBuilder = Builders<ObjectLevel>.Filter;
            var userForcesFilter = userForcesBuilder.Eq(ObjectIdCs, currentUser.OId);
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
                    {
                        fieldGroupsFinal.Add(fieldGroup);
                    }
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
                var objectLevelFilter = objectLevelBuilder.Eq(FieldId, x)
                    & objectLevelBuilder.Eq(IsActive, true);
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
                {
                    _posts.Add(post);
                }
            });
            var _postIds = _posts.Select(x => x.OId);

            //Kết hợp với datasoure ban đầu9
            //dataSource = dataSource.Where(x => _postIds.Contains(x.OId)).Distinct().ToList();
            dataSource.AddRange(_posts);
            dataSource = dataSource.GroupBy(x => x.OId).Select(grp => grp.FirstOrDefault()).ToList();

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                dataSource = dataSource.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
            }

            return mapper.Map<List<PostViewModel>>(dataSource.Where(x=>x.Status ==ItemStatus.Active));
        }

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


        private async Task<List<User>> GetUsersMatchPostField(Post post)
        {
            var postObjectLevelBuilder = Builders<ObjectLevel>.Filter;
            var postObjectLevelFilter = postObjectLevelBuilder.Eq(ObjectIdCs, post.OId)
                & postObjectLevelBuilder.Eq(IsActive, true);
            var postObjectLevels = await objectLevelRepository.FindListAsync(postObjectLevelFilter);
            var allUsers = userRepository.GetAll();

            var result = new List<User>();
            foreach (var user in allUsers)
            {
                if ((await IsUserMatchListObjectLevel(user, postObjectLevels)))
                {
                    result.Add(user);
                }
            }
            return result;

        }

        private async Task<bool> IsUserMatchListObjectLevel(User user, List<ObjectLevel> objectLevels)
        {
            var userObjectLevelBuilder = Builders<ObjectLevel>.Filter;
            var userObjectLevelFilter = userObjectLevelBuilder.Eq(ObjectIdCs, user.OId)
                & userObjectLevelBuilder.Eq(IsActive, true);
            var userObjectLevels = await objectLevelRepository.FindListAsync(userObjectLevelFilter);
            return userObjectLevels.Select(userObjectLevel => objectLevels.FirstOrDefault(x => x.FieldId == userObjectLevel.FieldId && x.LevelId == userObjectLevel.LevelId)).Any(_ => _ != null);
        }

        private bool IsMatch(PostViewModel post, LevelFilterItem levelFilterItem)
        {
            try
            {
                if (string.IsNullOrEmpty(levelFilterItem.FieldId) || string.IsNullOrEmpty(levelFilterItem.LevelId))
                {
                    return true;
                }

                var objlvls = objectLevelRepository.GetAll().Where(x => x.ObjectId == post.OId);

                foreach (var item in objlvls)
                {
                    var lvlFilter = levelRepository.GetAll().FirstOrDefault(x => x.OId == levelFilterItem.LevelId);
                    var lvlPost = levelRepository.GetAll().FirstOrDefault(x => x.OId == item.LevelId);

                    if (item.FieldId == levelFilterItem.FieldId && lvlFilter.Order <= lvlPost.Order)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private bool IsMatch(PostViewModel post, LevelFilter levelFilter)
        {
            try
            {
                foreach (var item in levelFilter.FilterItems)
                {
                    if (IsMatch(post, item) == true)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public async Task<PostViewModel> ModifiedPostStatus(ModifedPostStatusRequest request)
        {
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (post == null)
                throw new Exception("Không tìm thấy bài viết. ");
            post.Status = request.Status;
            await postRepository.UpdateAsync(post, post.Id);
            return mapper.Map<PostViewModel>(post);
        }
    }
}
