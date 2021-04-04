using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Identity.Services.GoogleAuthService
{
    /// <summary>
    /// Interface IGoogleAuthServices
    /// </summary>
    public interface IGoogleAuthServices
    {
        /// <summary>
        /// Externals the login.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        Task<object> ExternalLogin(GoogleAuthenticationRequest request, string ipAddress);
    }
}
