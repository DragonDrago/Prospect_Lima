using System.ComponentModel.DataAnnotations;

namespace Lima.Admin.Api.Requests
{
    public class MarkupRequest
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название наценки.")]
        public string Name { get; set; }
        public decimal MarkupPercent { get; set; }
        public decimal Percent { get; set; }
    }
}
