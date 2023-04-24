using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Notification
{
    internal class NotificationSender : INotificationSender
    {
        private NotificationServiceOptions _notificationServiceOptions;
        private IHttpContextAccessor _httpContextAccessor;

        public NotificationSender(NotificationServiceOptions notificationServiceOptions, IHttpContextAccessor httpContextAccessor)
        {
            _notificationServiceOptions = notificationServiceOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendAsync(string methodName)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(_notificationServiceOptions.Url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
                    options.SkipNegotiation = true;
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                    options.UseDefaultCredentials = true;
                })
                .Build();

            await connection.StartAsync();

            await connection.SendAsync(methodName);

            await connection.StopAsync();
        }
    }
}
