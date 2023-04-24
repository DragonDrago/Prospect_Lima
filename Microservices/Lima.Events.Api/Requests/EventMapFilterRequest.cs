using Microsoft.AspNetCore.Mvc;
using System;

namespace Lima.Events.Api.Requests
{
    public class EventMapFilterRequest
    {
        [FromQuery(Name = "user_ids")]
        public int[] UserIds { get; set; }

        [FromQuery(Name = "dates")]
        public DateTime[] Dates { get; set; }
    }
}
