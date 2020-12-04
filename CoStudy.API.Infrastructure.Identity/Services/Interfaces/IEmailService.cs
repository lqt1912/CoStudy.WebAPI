using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Identity.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
