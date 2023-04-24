using System;
using System.ComponentModel.DataAnnotations;

namespace Lima.Incomes.Api.Requests
{
    public class IncomeDetailingRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Выберите товар из списка.")]
        public int DrugId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите производителя из списка.")]
        public int ProducerId { get; set; }
        public int StoreId { get; set; }
        public DateTime ExpireDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Укажите количество упаковок.")]
        public int Quantity { get; set; }
    }
}
