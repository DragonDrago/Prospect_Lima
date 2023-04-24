using System.Collections.Generic;

namespace Lima.Messaging.Api.Domain
{
    public class Chat
    {
        public int? ChatId { get; set; }
        public string ChatName { get; set; }
        public int ChatTypeId { get; set; }
        public string ChatTypeName { get; set; }
        public List<ChatMember> ChatMembers { get; set; }

        public Chat()
        {
            ChatMembers = new List<ChatMember>();
        }
    }
}
