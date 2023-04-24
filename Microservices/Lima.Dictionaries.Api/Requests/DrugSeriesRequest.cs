using System;
using System.ComponentModel.DataAnnotations;

namespace Lima.Dictionaries.Api.Requests
{
    public class DrugSeriesRequest
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите товар из списка.")]
        public int DrugId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите серию твоара.")]
        public string Series { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Укажите количество.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Укажите срок годности.")]
        public DateTime ExpiredDate { get; set; }
    }
}
