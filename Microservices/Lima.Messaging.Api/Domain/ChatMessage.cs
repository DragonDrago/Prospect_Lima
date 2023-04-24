using System;

namespace Lima.Messaging.Api.Domain
{
    public class ChatMessage
    {
        public int? ChatId { get; set; }
        public string ChatName { get; set; }
        public int MessageId { get; set; }
        public int ChatTypeId { get; set; }
        public string Text { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public DateTime DateCreate { get; set; }
        public bool Read { get; set; }
    }
}
