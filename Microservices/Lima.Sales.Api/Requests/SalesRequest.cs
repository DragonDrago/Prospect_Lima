using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lima.Sales.Api.Requests
{
    public class SalesRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Укажите номер брони.")]
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int PrepaymentPercent { get; set; }
        public List<SalesDetailingRequest> SalesDetailing { get; set; }  
    }
}
