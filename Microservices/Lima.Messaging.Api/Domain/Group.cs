using System.Collections.Generic;

namespace Lima.Messaging.Api.Domain
{
    public class Group
    {
        public int CreationUserId { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<int> MembersIds { get; set; }
    }
}
