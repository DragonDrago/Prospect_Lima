using Chat.Client.Domain;
using Chat.Client.View;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Chat.Client.Presenter
{
    public class ChatPresenter : IPresenter
    {
        public event Action Close;

        private IChatView _view;
        private HubConnection _connection;
        private List<ChatItem> _chats;
        private ChatItem _selectedChat;

        public ChatPresenter(IChatView view)
        {
            _view = view;

            _view.Loaded += View_Loaded;
            _view.ChatSelected += View_ChatSelected;
            _view.SendMessage += View_SendMessage;
            _view.WindowClosed += View_WindowClosed;
        }

        private void View_WindowClosed()
        {
            Close?.Invoke();
        }

        private async void View_SendMessage(string message)
        {
            if (_selectedChat == null)
                return;

            try
            {
                await _connection.InvokeAsync("SendPrivateMessage",
                    _selectedChat.ChatId, _selectedChat.ChatMembers.FirstOrDefault().UserId, message);
            }
            catch (Exception ex)
            {
                //listBox2.Items.Add(ex.Message);
            }
        }

        private async void View_ChatSelected(ChatItem obj)
        {
            _selectedChat = obj;

            if (obj.ChatId == null)
                return;

            _view.SetChatMessages(await AppService.ChatRepository.GetChatMessagesAsync((int)obj.ChatId));
        }

        private async void View_Loaded()
        {
            InitializeHubConnection();

            _chats = (await AppService.ChatRepository.GetChatsAsync()).ToList();
            _view.SetChats(_chats);
        }

        private async void InitializeHubConnection()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5012/chatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AppGlobal.Account.AccessToken);
                    options.SkipNegotiation = true;
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                    options.UseDefaultCredentials = true;
                })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string>("ReceiveMessage", (message) =>
            {
                ChatMessage? chatMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatMessage>(message);

                if (_chats.Exists(e => e.ChatMembers.Exists(e1 => e1.UserId == chatMessage.SenderId && e.ChatId == null)))
                {
                    int chatIndex = _chats.FindIndex(f => f.ChatMembers.Exists(e => e.UserId == chatMessage.SenderId));

                    _chats[chatIndex].ChatId = chatMessage.ChatId;
                }

                _view.ReceiveMessage(chatMessage.Text);
            });

            _connection.On<string>("UpdateChat", (message) =>
            {
                ChatMessage? chatMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatMessage>(message);

                if (_chats.Exists(e => e.ChatMembers.Exists(e1 => e1.UserId == chatMessage.ReceiverId && e.ChatId == null)))
                {
                    int chatIndex = _chats.FindIndex(f => f.ChatMembers.Exists(e => e.UserId == chatMessage.ReceiverId));

                    _chats[chatIndex].ChatId = chatMessage.ChatId;
                }

                _view.ReceiveMessage(chatMessage.Text);
            });

            _connection.On<string>("MemberConnected", (message) =>
            {
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(message);

                Message.ShowMessage($"{result.user_id} connected!");
            });

            
            await _connection.StartAsync();
        }


        public DialogResult Run() => _view.Show();
    }
}
