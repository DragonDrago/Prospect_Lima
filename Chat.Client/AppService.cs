using Chat.Client.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client
{
    public static class AppService
    {
        public static IAuthRepository AuthenticateRepository { get; }
        public static IChatRepository ChatRepository { get; }

        static AppService()
        {
            AuthenticateRepository = new AuthRepository();
            ChatRepository = new ChatRepository();
        }
    }
}
