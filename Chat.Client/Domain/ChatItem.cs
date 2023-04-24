using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Domain
{
    public class ChatItem
    {
        [JsonProperty("chat_id")]
        public int? ChatId { get; set; }
        public string ChatName { get; set; }

        [JsonProperty("chat_type_id")]
        public int ChatTypeId { get; set; }
        public string ChatTypeName { get; set; }

        [JsonProperty("chat_members")]
        public List<ChatMember> ChatMembers { get; set; }

        public override string ToString()
        {
            return ChatName ?? ChatMembers.FirstOrDefault().UserName;
        }
    }
}
