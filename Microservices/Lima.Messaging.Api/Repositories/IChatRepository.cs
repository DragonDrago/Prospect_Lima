using Lima.Core.Tenant;
using Lima.Messaging.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lima.Messaging.Api.Repositories
{
    public interface IChatRepository
    {
        public ITenantContext TenantContext { get; set; }
        Task<IEnumerable<Chat>> GetChatsAsync(int userId);
        Task<Chat> GetChatInfoAsync(int userId, int chatId);
        Task<MessageResult> SendMessageAsync(ChatMessage chatMessage);
        Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId, int userId, int offset);
    }
}
