using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class ReplyCommentRepository : BaseRepository<ReplyComment>, IReplyCommentRepository
    {
        IConfiguration configuration;
        public ReplyCommentRepository(IConfiguration configuration) : base("reply_comment", configuration)
        {
            this.configuration = configuration;
        }
    }
}
