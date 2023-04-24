using System;

namespace Lima.Events.Api.Domain
{
    public class EventMapFilter
    {
        public int[] UserIds { get; set; }
        public DateTime[] Dates { get; set; }
    }
}
