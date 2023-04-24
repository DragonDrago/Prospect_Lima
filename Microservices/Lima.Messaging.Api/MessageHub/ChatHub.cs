using Lima.Core.Json;
using Lima.Core.Tenant;
using Lima.Messaging.Api.Domain;
using Lima.Messaging.Api.Repositories;
using Lima.Messaging.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lima.Messaging.Api.MessageHub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        public static Dictionary<string, List<ChatMember>> ChatMembers { get; }

        private IChatRepository _chatRepository;
        private ITenantService _tenantService;
        private IHttpContextAccessor _httpContextAccessor;

        static ChatHub()
        {
            ChatMembers = new Dictionary<string, List<ChatMember>>();
        }

        public ChatHub(IChatRepository chatRepository, ITenantService tenantService, IHttpContextAccessor httpContextAccessor)
        {
            _chatRepository = chatRepository;
            _tenantService = tenantService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessageToGroup(int chatId, string message)
        {
            try
            {
                int senderId = Convert.ToInt32(Context.User.Claims.FirstOrDefault(f => f.Type == "id").Value);

                //сохранение сообщения в БД
                var messageResult = await _chatRepository.SendMessageAsync(new ChatMessage() 
                { 
                    ChatId = chatId,
                    ChatTypeId = 2,
                    SenderId = senderId,
                    Text = message
                });

                //находим группу в памяти
                var group = Clients.Group($"{chatId}");

                //если группы нет в памяти, загружаем информацию из БД
                if (group == null)
                {
                    //извлекаем информацию о чате и список участников
                    var chatInfo = await _chatRepository.GetChatInfoAsync(senderId, chatId);

                    //сопоставление и извлечение connectionId
                    //var groupChatConnections = ChatMembers
                    //    .Where(w => chatInfo.ChatMembers.Exists(e => e.UserId == w.UserId))
                    //    .Select(s => s.ConnectionId);

                    ////добавление всех участников в группу
                    //foreach (var userGroupChatConnection in groupChatConnections)
                    //{
                    //    await Groups.AddToGroupAsync(userGroupChatConnection, $"{chatId}");
                    //}

                    group = Clients.Group($"{chatId}");
                }

                //рассылка сообщения всем участникам
                await group.SendAsync("sendMessage", new
                {
                    messageResult.ChatId,
                    messageResult.MessageId,
                    Text = message
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Возникла проблема с отправкой сообщения в группу" + ex.Message);
            }
        }

        public async Task SendPrivateMessage(int? chatId, int recieverId, string message)
        {
            try
            {
                string tenantId = Context.User.Claims.FirstOrDefault(f => f.Type == "tid").Value;

                int senderId = Convert.ToInt32(Context.User.Claims.FirstOrDefault(f => f.Type == "id").Value);

                IChatRepository chatRepository = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IChatRepository>();

                var tenantResult = await _tenantService.GetTenantInfoAsync(tenantId, senderId);

                if (tenantResult == null)
                    throw new Exception("Unknown person");

                chatRepository.TenantContext = new TenantContext()
                {
                    CurrentTenant = new Tenant()
                    {
                        ConnectionString = tenantResult.ConnectionString,
                    }
                };

                var messageResult = await chatRepository.SendMessageAsync(new ChatMessage()
                {
                    ChatId = chatId,
                    SenderId = senderId,
                    ReceiverId = recieverId,
                    ChatTypeId = 1,
                    Text = message
                });

                if (ChatMembers.ContainsKey(tenantId))
                {
                    if (ChatMembers[tenantId].Exists(e => e.UserId == recieverId)) 
                    {
                        ChatMember reciever = ChatMembers[tenantId].FirstOrDefault(f => f.UserId == recieverId);

                        await Clients.Client(reciever.ConnectionId).SendAsync("receiveMessage", SerializeData(new
                        {
                            messageResult.ChatId,
                            SenderId = senderId,
                            ReceiverId = recieverId,
                            messageResult.MessageId,
                            Text = message
                        }));
                    }
                    else
                    {
                        ChatMember sender = ChatMembers[tenantId].FirstOrDefault(f => f.UserId == senderId);

                        await Clients.Client(sender.ConnectionId).SendAsync("updateChat", SerializeData(new
                        {
                            messageResult.ChatId,
                            messageResult.MessageId,
                            SenderId = senderId,
                            ReceiverId = recieverId,
                            Text = message
                        }));
                    }
                }
               
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Возникла проблема с отправкой личного сообщения" + ex.Message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            string tenantId = Context.User.Claims.FirstOrDefault(f => f.Type == "tId").Value;

            int userId = Convert.ToInt32(Context.User.Claims.FirstOrDefault(f => f.Type == "id").Value);

            var charMember = new ChatMember()
            {
                UserId = userId,
                ComapnyTenantId = Context.User.Claims.FirstOrDefault(f => f.Type == "tId").Value,
                ConnectionId = Context.ConnectionId
            };

            if (ChatMembers.ContainsKey(tenantId))
            {
                if (ChatMembers[tenantId].Exists(e => e.UserId == userId))
                {
                    ChatMember chatMember = ChatMembers[tenantId].FirstOrDefault(f => f.UserId == userId);
                    chatMember.ConnectionId = Context.ConnectionId;
                }
                else
                {
                    ChatMembers[tenantId].Add(charMember);
                }
            }
            else
            {
                var tenantResult = await _tenantService.GetTenantInfoAsync(tenantId, userId);

                if (tenantResult == null)
                    throw new Exception("Unknown person");

                ChatMembers[tenantId] = new List<ChatMember>() { charMember };
            }

            foreach (var chatMemberItem in ChatMembers[tenantId].Where(w => w.UserId != userId))
            {
                await Clients.Client(chatMemberItem.ConnectionId).SendAsync("memberConnected", SerializeData(new
                {
                    UserId = userId
                }));
            }

            await base.OnConnectedAsync();
        }


        private string SerializeData(object data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            });
        }
    }
}
