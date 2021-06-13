using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface IConversationService
    {
             Task<ConversationItemTypeViewModel> AddConversationItemType(ConversationItemType entity);


             Task<ConversationItemTypeViewModel> GetItemTypeByCode(string code);


             Task<ConversationViewModel> AddConversation(AddConversationRequest request);

            Task<GetConversationByUserIdResponse> GetConversationByUserId();

             Task<IEnumerable<MessageViewModel>> AddMember(AddMemberRequest request);

             Task<string> DeleteConversation(string id);
    }

}
