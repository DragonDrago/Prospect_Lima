using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Domain
{
    public class ChatMember
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }
        public string ComapnyTenantId { get; set; }
        public string ConnectionId { get; set; }
    }
}
