using Chat.Client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Repository
{
    public interface IAuthRepository
    {
        Task<Account> Login(string username, string password);
    }
}
