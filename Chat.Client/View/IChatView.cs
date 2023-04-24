using Chat.Client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.View
{
    public interface IChatView : IView
    {
        event Action Loaded;
        event Action<ChatItem> ChatSelected;
        event Action<string> SendMessage;
        event Action WindowClosed;

        void ReceiveMessage(string message);
        void SetChatMessages(IEnumerable<ChatMessage> chatMessages);
        void SetChats(IEnumerable<ChatItem> chatItems);
        void Notify(int userId);
    }
}
