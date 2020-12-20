using CoStudy.API.Domain.Entities.Application;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse
{
    public class AddFieldResponse
    {
        public string UserId { get; set; }
        public Field Field { get; set; }
    }
}
