using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Notification
{
    public static class Extensions
    {
        public static IServiceCollection AddNotificationService(this IServiceCollection services, Action<NotificationServiceOptions> options)
        {
            services.AddOptions<NotificationServiceOptions>()
                .Configure(options);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<INotificationSender, NotificationSender>();

            return services;
        }
    }
}
