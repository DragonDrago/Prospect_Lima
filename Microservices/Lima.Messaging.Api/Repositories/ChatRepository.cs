using Dapper;
using Lima.Core.Tenant;
using Lima.Messaging.Api.Domain;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Messaging.Api.Repositories
{
    public class ChatRepository : IChatRepository
    {
        public ITenantContext TenantContext { get; set; }

        public string ConnectionString { get; set; }

        public ChatRepository(ITenantContext tenantContext)
        {
            TenantContext = tenantContext;
        }

        /// <summary>
        /// Создаёт групповой чат.
        /// </summary>
        /// <param name="group">Объект группового чата.</param>
        /// <returns></returns>
        public async Task<int> CreateGroupAsync(Group group)
        {
            using (SqlConnection connection = new SqlConnection(TenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int chatId = await CreateChatAsync(
                            2,
                            group.CreationUserId,
                            group.GroupName,
                            group.MembersIds,
                            connection,
                            transaction);

                        await transaction.CommitAsync();

                        return chatId;
                    }
                    catch (System.Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Сохраняет приватное сообщение.
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        public async Task<MessageResult> SendMessageAsync(ChatMessage chatMessage)
        {
            using (SqlConnection connection = new SqlConnection(TenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (chatMessage.ChatId == null || chatMessage.ChatId == 0)
                        {
                            List<int> chatMemberIds = new List<int> 
                            { 
                                chatMessage.SenderId, 
                                (int)chatMessage.ReceiverId
                            };

                            chatMessage.ChatId = await CreateChatAsync(
                                chatMessage.ChatTypeId, 
                                chatMessage.SenderId, 
                                chatMessage.ChatName,
                                chatMemberIds, 
                                connection, 
                                transaction);
                        }

                        int messageId = await connection.QueryFirstOrDefaultAsync<int>(
                            sql: "dbo.insertMessage",
                            param: new
                            {
                                @chatId = chatMessage.ChatId,
                                @text = chatMessage.Text,
                                @senderId = chatMessage.SenderId,
                                @receiverId = chatMessage.ReceiverId
                            },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        await transaction.CommitAsync();

                        return new MessageResult()
                        {
                            ChatId = (int)chatMessage.ChatId,
                            MessageId = messageId
                        };
                    }
                    catch (System.Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Получает список чатов.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Chat>> GetChatsAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(TenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                List<Chat> chats = new List<Chat>();

                await connection.QueryAsync<Chat, ChatMember, Chat>(
                    sql: "dbo.getChats",
                    param: new { @userId },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "UserId",
                    map: (c, cm) =>
                    {
                        if (c.ChatId == null)
                        {
                            c.ChatMembers.Add(cm);
                            chats.Add(c);

                            return c;
                        }

                        int index = chats.FindIndex(f => f.ChatId == c.ChatId);

                        if (index == -1)
                        {
                            chats.Add(c);

                            index = chats.Count - 1;
                        }

                        chats[index].ChatMembers.Add(cm);

                        return c;
                    });

                return chats;
            }
        }

        /// <summary>
        /// Получает информацию о чате.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<Chat> GetChatInfoAsync(int userId, int chatId)
        {
            return null;
        }

        /// <summary>
        /// Создаёт новый чат.
        /// </summary>
        /// <param name="chatTypeId">Тип чата.</param>
        /// <param name="chatCreationUserId">Id пользователя, который был инициатором создания чата.</param>
        /// <param name="chatName">Название чата.</param>
        /// <param name="memberIds">Список Id пользователей, которые относятся к данному чату.</param>
        /// <param name="sqlConnection"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        private async Task<int> CreateChatAsync(
            int chatTypeId, 
            int chatCreationUserId, 
            string chatName,
            IEnumerable<int> memberIds, 
            SqlConnection sqlConnection, 
            SqlTransaction sqlTransaction)
        {
            DataTable dataTable = new DataTable("CHAT_MEMBER");
            dataTable.Columns.Add("UserId");

            foreach (var memberId in memberIds)
            {
                dataTable.Rows.Add(memberId);
            }
            
            return await sqlConnection.QueryFirstOrDefaultAsync<int>(
                            sql: "dbo.createChat",
                            param: new 
                            { 
                                @chatTypeId,
                                @chatCreationUserId,
                                @chatName,
                                @members = dataTable.AsTableValuedParameter()
                            },
                            commandType: System.Data.CommandType.StoredProcedure,
                            transaction: sqlTransaction);
        }

        /// <summary>
        /// Получает сообщения чата.
        /// </summary>
        /// <param name="chatId">Id чата.</param>
        /// <param name="userId">Id пользоватлея, который запрашивает сообщения.</param>
        /// <param name="offset">Смещение.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId, int userId, int offset)
        {
            using (SqlConnection connection = new SqlConnection(TenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<ChatMessage>(
                    sql: "dbo.getChatMessages",
                    param: new { @chatId, @userId, @offset },
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
