using System;

namespace Lima.Events.Api.Domain
{
    public class EventMap
    {
        public int? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Organization Organization { get; set; }
        public Visit Visit { get; set; }
    }
}
