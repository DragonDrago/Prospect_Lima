using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Domain
{
    public class ChatMessage
    {
        [JsonProperty("chat_id")]
        public int? ChatId { get; set; }

        [JsonProperty("chat_name")]
        public string ChatName { get; set; }

        [JsonProperty("message_id")]
        public int MessageId { get; set; }

        [JsonProperty("chat_type_id")]
        public int ChatTypeId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("receiver_id")]
        public int? ReceiverId { get; set; }

        [JsonProperty("receiver_name")]
        public string ReceiverName { get; set; }

        [JsonProperty("sender_id")]
        public int SenderId { get; set; }

        [JsonProperty("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("date_create")]
        public DateTime DateCreate { get; set; }

        [JsonProperty("read")]
        public bool Read { get; set; }

        public override string ToString()
        {
            return $"[{SenderName}]: {Text}";
        }
    }
}
