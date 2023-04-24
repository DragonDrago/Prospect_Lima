using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lima.Events.Api.Requests
{
    public class EventRequest
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите организацию.")]
        public int OrganizationId { get; set; }
        public int? TypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RepeatDays { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите цвет события.")]
        public int ColorId { get; set; }
        public string Comment { get; set; }
        public int[] Members { get; set; }
        public int[] Drugs { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorPhone { get; set; }
        public bool IsTask { get; set; }
    }
}
