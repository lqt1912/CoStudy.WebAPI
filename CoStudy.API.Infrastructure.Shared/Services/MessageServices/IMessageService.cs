using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
       public interface IMessageService
    {
             Task<MessageViewModel> AddMessage(AddMessageRequest request);

             Task<IEnumerable<MessageViewModel>> GetMessageByConversationId(GetMessageByConversationIdRequest request);

            List<Message> GetAll();
             Task<string> DeleteMessage(string id);
             Task<MessageViewModel> EditMessage(UpdateMessageRequest request);
    }
}
