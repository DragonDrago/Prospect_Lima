using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lima.Dictionaries.Api.Requests
{
    public class DrugRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара.")]
        public string DrugName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Выберите производителя.")]
        public int ProducerId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Выберите страну производителя.")]
        public int CountryId { get; set; }
        public decimal BasePrice { get; set; }
        public int? StorePlaceId { get; set; }
        public string StorePlaceName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int Amount { get; set; }
        public int? UnitId { get; set; }
        public string Photo { get; set; }
        public string PhotoName { get; set; }
    }
}
