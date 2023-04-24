using System.Collections.Generic;

namespace Lima.Visits.Api.Dto
{
    public class NewOrderDto
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public decimal PrepaymentPercent { get; set; }
        public List<NewOrderDetailingDto> OrdersDetailing { get; set; }
    }
}
