using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Hangfire
{
    public interface IHangfireService
    {
        string RemoveViolencePost();
        string RemoveViolenceComment();
        string RemoveViolenceReply();
    }
}
