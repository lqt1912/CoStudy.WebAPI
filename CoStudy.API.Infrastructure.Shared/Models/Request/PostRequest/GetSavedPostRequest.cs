using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class GetSavedPostRequest : BaseGetAllRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public OrderType OrderType  { get; set; }
    }

    public enum OrderType
    {
        Ascending,
        Descending
    }
}
