using Chat.Client.Domain;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Chat.Client.Repository
{
    public class ChatRepository : IChatRepository
    {
        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId)
        {
            return await Program.BASE_URL
                .AppendPathSegments("chats", $"{chatId}", "messages")
                .WithHeader("Authorization", "Bearer " + AppGlobal.Account.AccessToken)
                .GetAsync()
                .ReceiveJson<IEnumerable<ChatMessage>>();
        }

        public async Task<IEnumerable<ChatItem>> GetChatsAsync()
        {
            return await Program.BASE_URL
                .AppendPathSegments("chats", "all")
                .WithHeader("Authorization", "Bearer " + AppGlobal.Account.AccessToken)
                .GetAsync()
                .ReceiveJson<IEnumerable<ChatItem>>();
        }
    }
}
