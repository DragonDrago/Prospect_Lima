using System.ComponentModel.DataAnnotations;

namespace Lima.Visits.Api.Requests
{
    public class CompleateVisitRequest
    {
        [Range(1, int.MaxValue)]
        public int VisitId { get; set; }

        [Range(-180, 180, ErrorMessage = "Неверные координаты.")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Неверные координаты.")]
        public double Longitude { get; set; }
    }
}
