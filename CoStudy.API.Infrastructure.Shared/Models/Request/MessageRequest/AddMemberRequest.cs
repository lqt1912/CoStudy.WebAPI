using CoStudy.API.Infrastructure.Shared.Validator;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class AddMemberRequest
    {
              [StringRequired]
        public string ConversationId { get; set; }

              [ListRequired]
        public IEnumerable<string> UserIds { get; set; }
    }
}
