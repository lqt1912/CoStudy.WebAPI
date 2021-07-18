using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class NewsFeedQuery
    {
        public bool OnlyFollow { get; set; }
        public bool OnlyPersonalField { get; set; }
        public _ArrangeType? Arrange { get; set; }
    }

    public enum _ArrangeType
    {
        CreatedDate,
        Random
    }
}
