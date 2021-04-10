using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    /// <summary>
    /// The MessageService Interface
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<MessageViewModel> AddMessage(AddMessageRequest request);

        /// <summary>
        /// Gets the message by conversation identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<MessageViewModel>> GetMessageByConversationId(GetMessageByConversationIdRequest request);
      
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        List<Message> GetAll();
        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> DeleteMessage(string id);
        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<MessageViewModel> EditMessage(UpdateMessageRequest request);
    }
}
