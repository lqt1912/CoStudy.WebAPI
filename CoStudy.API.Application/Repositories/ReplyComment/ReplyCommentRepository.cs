using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class ReplyCommentRepository : BaseRepository<ReplyComment>, IReplyCommentRepository
    {
        public ReplyCommentRepository() : base("reply_comment")
        {

        }
    }
}
