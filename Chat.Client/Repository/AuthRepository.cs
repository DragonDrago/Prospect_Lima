using Chat.Client.Domain;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<Account> Login(string username, string password)
        {
            return await Program.BASE_URL
                .AppendPathSegments("users", "authenticate")
                .SetQueryParam("login", username)
                .SetQueryParam("password", password)
                .PostAsync()
                .ReceiveJson<Account>();
        }
    }
}
