using System;
using System.Collections.Generic;

#nullable disable

namespace Lima.Orders.Api.Models
{
    public partial class Visit
    {
        public Visit()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreate { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? DistributorId { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorPhone { get; set; }
        public int? PrepaymentPercent { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
