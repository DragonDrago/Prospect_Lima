using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Notification
{
    public interface INotificationSender
    {
        Task SendAsync(string method);
    }
}
