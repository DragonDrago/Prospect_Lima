using Chat.Client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Repository
{
    public interface IChatRepository
    {
        Task<IEnumerable<ChatItem>> GetChatsAsync();
        Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId);
    }
}
