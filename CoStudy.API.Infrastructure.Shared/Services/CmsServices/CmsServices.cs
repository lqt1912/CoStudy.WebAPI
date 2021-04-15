using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.ICmsServices" />
    public class CmsServices : ICmsServices
    {

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        IAccountRepository accountRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The external login repository
        /// </summary>
        IExternalLoginRepository externalLoginRepository;

        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="CmsServices"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        public CmsServices(IUserRepository userRepository, IMapper mapper, IAccountRepository accountRepository, IExternalLoginRepository externalLoginRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.accountRepository = accountRepository;
            this.externalLoginRepository = externalLoginRepository;
            this.postRepository = postRepository;
        }

        /// <summary>
        /// Gets the user paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TableResultJson<UserViewModel> GetUserPaged(TableRequest request)
        {
            var dataSource = userRepository.GetAll().OrderByDescending(x => x.CreatedDate).AsEnumerable();
            
            var dataSourceViewModels = mapper.Map<List<UserViewModel>>(dataSource).AsEnumerable();

            TableResultJson<UserViewModel> response = new TableResultJson<UserViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModels.Count();

            if(request.columns[1].search!=null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.FullName.Contains(request.columns[1].search.value));
            }

            if(request.columns[2].search!=null)
            {
                if (!string.IsNullOrEmpty(request.columns[2].search.value))
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.Email.Contains(request.columns[2].search.value));
            }

            if(request.columns[3].search!=null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                    dataSourceViewModels = dataSourceViewModels.Where(x => x.PhoneNumber.Contains(request.columns[3].search.value));
            }

            if(request.columns[4].search!=null)
            {
                if (!string.IsNullOrEmpty(request.columns[4].search.value))
                    dataSourceViewModels = dataSourceViewModels.Where(x => (x.FullAddress.Contains(request.columns[4].search.value)));
            }
            dataSourceViewModels = dataSourceViewModels.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModels.ToList();

            foreach (UserViewModel item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }
            return response;
        }


        /// <summary>
        /// Gets the post paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TableResultJson<PostViewModel> GetPostPaged(TableRequest request)
        {
            var dataSource = postRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToList();

            var dataSourceViewModel = mapper.Map<List<PostViewModel>>(dataSource).AsEnumerable();

            if(request.columns[1].search !=null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.AuthorName.Contains(request.columns[1].search.value));
            }

            if(request.columns[2].search!=null)
            {
                if (!string.IsNullOrEmpty(request.columns[2].search.value))
                    dataSourceViewModel = dataSourceViewModel.Where(x => x.Title.Contains(request.columns[2].search.value));
            }

            TableResultJson<PostViewModel> response = new TableResultJson<PostViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();

            foreach (PostViewModel item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }
            return response;
        }

            /// <summary>
            /// Gets the by email.
            /// </summary>
            /// <param name="email">The email.</param>
            /// <returns></returns>
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
    }
}
