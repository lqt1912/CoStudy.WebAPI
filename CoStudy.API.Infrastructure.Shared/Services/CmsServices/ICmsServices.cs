using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface ICmsServices
    {
             TableResultJson<UserViewModel> GetUserPaged(TableRequest request);

             Task<UserProfileViewModel> GetByEmail(string email);

             TableResultJson<PostViewModel> GetPostPaged(TableRequest request);

             TableResultJson<CommentViewModel> GetCommentPaged(TableRequest request);

             TableResultJson<ReplyCommentViewModel> GetReplyCommentPaged(TableRequest request);

             TableResultJson<ReportViewModel> GetReportPaged(TableRequest request);

             Task<CommentViewModel> GetCommentById(string id);

             Task<ReplyCommentViewModel> GetReplyCommentById(string id);

             Task<PostViewModel> GetPostById(string id);
    }
}
