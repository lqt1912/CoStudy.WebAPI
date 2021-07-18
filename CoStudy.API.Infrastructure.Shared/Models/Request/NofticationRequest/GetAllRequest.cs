using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class GetAllNotificationRequest :BaseGetAllRequest
    {
        public bool? IsRead { get; set; }
    }
}
