using Lima.Messaging.Api.Domain;
using Lima.Messaging.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lima.Messaging.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private IChatRepository _chatRepository;
        private ClaimsPrincipal _claimsPrincipal;
        private int _userId;

        public ChatsController(IChatRepository chatRepository, ClaimsPrincipal claimsPrincipal)
        {
            _chatRepository = chatRepository;
            _claimsPrincipal = claimsPrincipal;

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id");

            if (userIdClaim == null)
            {
                ModelState.AddModelError("UserError", "Ошибка аутентификации");
            }
            else
            {
                _userId = Convert.ToInt32(userIdClaim.Value);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> Chats()
        {
            int userId = Convert.ToInt32(_claimsPrincipal.Claims.FirstOrDefault(f => f.Type == "id").Value);

            return Ok(await _chatRepository.GetChatsAsync(userId));
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> Messages(int? chatId, int offset)
        {
            if (chatId == 0 || chatId == null)
                return Ok(Enumerable.Empty<ChatMessage>());

            return Ok(await _chatRepository.GetChatMessagesAsync((int)chatId, _userId, offset));
        }
    }
}
