﻿using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class UpVoteRepository : BaseRepository<UpVote>, IUpVoteRepository
    {
        public UpVoteRepository() : base("upvote")
        {

        }
    }

    public class DownVoteRepository : BaseRepository<DownVote>, IDownVoteRepository
    {
        public DownVoteRepository() : base("downvote")
        {

        }
    }
}
