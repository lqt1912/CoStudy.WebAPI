using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Interface ICmsServices
    /// </summary>
    public interface ICmsServices
    {
        /// <summary>
        /// Gets the user paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TableResultJson<UserViewModel> GetUserPaged(TableRequest request);

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<UserProfileViewModel> GetByEmail(string email);

        /// <summary>
        /// Gets the post paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TableResultJson<PostViewModel> GetPostPaged(TableRequest request);
    }
}
