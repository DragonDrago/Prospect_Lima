using Lima.Notification.Api.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.Notification.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        public Task<IActionResult> Add([FromBody] NotificationRequest notificationRequest)
        {
            return null;
        }
    }
}
