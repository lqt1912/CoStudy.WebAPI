using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Interface IConversationService
    /// </summary>
    public interface IConversationService
    {
        /// <summary>
        /// Adds the type of the conversation item.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<ConversationItemTypeViewModel> AddConversationItemType(ConversationItemType entity);


        /// <summary>
        /// Gets the item type by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task<ConversationItemTypeViewModel> GetItemTypeByCode(string code);


        /// <summary>
        /// Adds the conversation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ConversationViewModel> AddConversation(AddConversationRequest request);

        /// <summary>
        /// Gets the conversation by user identifier.
        /// </summary>
        /// <returns></returns>
        Task<GetConversationByUserIdResponse> GetConversationByUserId();

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<MessageViewModel>> AddMember(AddMemberRequest request);

        /// <summary>
        /// Deletes the conversation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> DeleteConversation(string id);
    }

}
