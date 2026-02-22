using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class GetPostByUserRequest : BaseGetAllRequest
    {
        public string UserId { get; set; }
    }
}
