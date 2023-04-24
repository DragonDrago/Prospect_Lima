namespace Lima.Messaging.Api.Domain
{
    public class ChatMember
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ComapnyTenantId { get; set; }
        public string ConnectionId { get; set; }
    }
}
