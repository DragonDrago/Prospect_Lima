using System;
using System.Collections.Generic;

namespace Lima.Events.Api.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public int? VisitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventColor Color { get; set; }
        public string Comment { get; set; }
        public int RepeatDays { get; set; }
        public bool IsTask { get; set; }
        public int UserCreateId { get; set; }
        public EventDoctor EventDoctor { get; set; }
        public List<EventMember> EventMembers { get; set; }
        public List<EventDrug> TalkedAboutTheDrugs { get; set; }
    }
}
