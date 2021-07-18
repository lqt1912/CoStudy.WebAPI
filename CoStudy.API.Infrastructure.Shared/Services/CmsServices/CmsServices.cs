using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class CmsServices : ICmsServices
    {
        IUserRepository userRepository;

        IAccountRepository accountRepository;

        IMapper mapper;

        IExternalLoginRepository externalLoginRepository;

        IPostRepository postRepository;

        ICommentRepository commentRepository;

        IReplyCommentRepository replyCommentRepository;

        private IReportRepository reportRepository;

        public CmsServices(IUserRepository userRepository, IMapper mapper, IAccountRepository accountRepository, IExternalLoginRepository externalLoginRepository, IPostRepository postRepository, ICommentRepository commentRepository, IReplyCommentRepository replyCommentRepository, IReportRepository reportRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.accountRepository = accountRepository;
            this.externalLoginRepository = externalLoginRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.reportRepository = reportRepository;
        }

        public TableResultJson<UserViewModel> GetUserPaged(TableRequest request)
        {
            var dataSource = userRepository.GetAll().OrderByDescending(x => x.CreatedDate).AsEnumerable();

            var dataSourceViewModels = mapper.Map<List<UserViewModel>>(dataSource).AsEnumerable();

            TableResultJson<UserViewModel> response = new TableResultJson<UserViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModels.Count();

            if (request.columns[1].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                {
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.FullName.Contains(request.columns[1].search.value));
                }
            }

            if (request.columns[2].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[2].search.value))
                {
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.Email.Contains(request.columns[2].search.value));
                }
            }

            if (request.columns[3].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                {
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.PhoneNumber.Contains(request.columns[3].search.value));
                }
            }

            if (request.columns[4].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[4].search.value))
                {
                    dataSourceViewModels = dataSourceViewModels.Where(x => (x.FullAddress.Contains(request.columns[4].search.value)));
                }
            }

            if (request.columns[9].search != null)
            {
                var _value = request.columns[9].search.value;
                if (!string.IsNullOrEmpty(_value))
                {
                    if (_value != "5")
                    {
                        var _status = ItemStatus.TryParse(_value, out ItemStatus __status);
                        dataSourceViewModels = dataSourceViewModels.Where(x => x.Status == __status);
                    }
                }
            }

            dataSourceViewModels = dataSourceViewModels.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModels.ToList();

            foreach (UserViewModel item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }
            return response;
        }

        public TableResultJson<ReportViewModel> GetReportPaged(TableRequest request)
        {
            var dataSource = reportRepository.GetAll().OrderByDescending(x=>x.CreatedDate);
            var dataSourceViewModel = mapper.Map<List<ReportViewModel>>(dataSource).AsEnumerable();
            var response = new TableResultJson<ReportViewModel>();

            if(request.columns[1].search!=null)
            {
                var _value1 = request.columns[1].search.value;
                if (!string.IsNullOrEmpty(_value1))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.OId.Contains(_value1));
            }

            if (request.columns[2].search != null)
            {
                var _value2 = request.columns[2].search.value;
                if (!string.IsNullOrEmpty(_value2))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.AuthorName.Contains(_value2));
            }

            if (request.columns[5].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[4].search.value))
                {
                    var dt = DateTime.TryParseExact(request.columns[4].search.value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
                    if (dt)
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.CreatedDate.Value.Date == datetime);
                }
            }

            if(request.columns[6].search!=null)
            {
                var _value3 = request.columns[6].search.value;
                if (!string.IsNullOrEmpty(_value3))
                {
                    if (_value3 == "true")
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.IsApproved == true);
                    else if (_value3 == "false")
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.IsApproved == false);
                }
            }

            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();
            foreach (var item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }

            return response;
        }
        public TableResultJson<PostViewModel> GetPostPaged(TableRequest request)
        {
            var dataSource = postRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToList();

            var dataSourceViewModel = mapper.Map<List<PostViewModel>>(dataSource).AsEnumerable();

            if (request.columns[1].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                {
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.AuthorName.Contains(request.columns[1].search.value));
                }
            }

            if (request.columns[2].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[2].search.value))
                {
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.Title.Contains(request.columns[2].search.value));
                }
            }
            if (request.columns[4].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[4].search.value))
                {
                    var dt = DateTime.TryParseExact(request.columns[4].search.value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
                    if (dt)
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.CreatedDate.Date == datetime);
                }
            }

            if (request.columns[9].search != null)
            {
                var _value = request.columns[9].search.value;
                if (!String.IsNullOrEmpty(_value) && _value!="All")
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.StatusName.Contains(_value));
            }

            TableResultJson<PostViewModel> response = new TableResultJson<PostViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();

            response.data.ForEach(x => { x.Index = response.data.IndexOf(x) + request.start + 1; });

            return response;
        }

        public TableResultJson<CommentViewModel> GetCommentPaged(TableRequest request)
        {
            var dataSource = commentRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToList();
            var dataSourceViewModel = mapper.Map<List<CommentViewModel>>(dataSource).AsEnumerable();

            if (request.columns[1].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.OId.Contains(request.columns[1].search.value));
            }

            if (request.columns[2].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[2].search.value))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.AuthorName.Contains(request.columns[2].search.value));
            }

            if (request.columns[3].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                {
                    var dt = DateTime.TryParseExact(request.columns[3].search.value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
                    if (dt)
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.CreatedDate.Value.Date == datetime);
                }
            }

            if (request.columns[10].search != null)
            {
                var _value = request.columns[10].search.value;
                if (!String.IsNullOrEmpty(_value))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.Content.Contains(_value));
            }

            TableResultJson<CommentViewModel> response = new TableResultJson<CommentViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();

            response.data.ForEach(x => { x.Index = response.data.IndexOf(x) + request.start + 1; });
            return response;
        }

        public TableResultJson<ReplyCommentViewModel> GetReplyCommentPaged(TableRequest request)
        {
            var dataSource = replyCommentRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToList();
            var dataSourceViewModel = (mapper.Map<List<ReplyCommentViewModel>>(dataSource)).AsEnumerable();

            if (request.columns[1].search != null)
            {
                var _value1 = request.columns[1].search.value;
                if (!string.IsNullOrEmpty(_value1))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.OId.Contains(_value1));
            }

            if (request.columns[2].search != null)
            {
                var _value2 = request.columns[2].search.value;
                if (!string.IsNullOrEmpty(_value2))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.AuthorName.Contains(_value2));
            }

            if (request.columns[3].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                {
                    var dt = DateTime.TryParseExact(request.columns[3].search.value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
                    if (dt)
                        dataSourceViewModel = dataSourceViewModel.Where(x => x.CreatedDate.Value.Date == datetime);
                }
            }

            if (request.columns[8].search != null)
            {
                var _value4 = request.columns[8].search.value;
                if (!string.IsNullOrEmpty(_value4))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.Content.Contains(_value4));
            }

            var response = new TableResultJson<ReplyCommentViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();

            response.data.ForEach(x => { x.Index = response.data.IndexOf(x) + request.start + 1; });
            return response;

        }

        public async Task<UserProfileViewModel> GetByEmail(string email)
        {
            var accountFilter = Builders<Account>.Filter.Eq("Email", email);
            var account = await accountRepository.FindAsync(accountFilter);

            var userFilter = Builders<User>.Filter.Eq("email", email);
            var user = await userRepository.FindAsync(userFilter);
            var userViewModel = mapper.Map<UserViewModel>(user);

            var externalFilter = Builders<ExternalLogin>.Filter.Eq("email", email);
            var external = await externalLoginRepository.FindAsync(externalFilter);

            var result = new UserProfileViewModel()
            {
                Account = account,
                ExternalLogin = external,
                User = userViewModel
            };
            return result;
        }

        public async Task<CommentViewModel> GetCommentById(string id)
        {
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(id));
            var result = mapper.Map<CommentViewModel>(comment);
            return result;
        }

        public async Task<ReplyCommentViewModel> GetReplyCommentById(string id)
        {
            var replyComment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(id));
            var result = mapper.Map<ReplyCommentViewModel>(replyComment);
            return result;
        }

        public async Task<PostViewModel> GetPostById(string id)
        {
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(id));
            var result = mapper.Map<PostViewModel>(post);
            return result;
        }
    }
}
