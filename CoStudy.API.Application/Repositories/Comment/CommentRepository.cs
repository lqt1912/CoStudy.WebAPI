using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        IConfiguration configuration;
        public CommentRepository(IConfiguration configuration) : base("comment", configuration)
        {
            this.configuration = configuration;
        }
    }
}
