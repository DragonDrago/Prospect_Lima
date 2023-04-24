using System;

namespace Lima.Incomes.Api.Model
{
    public class SaveIncomeDetailing
    {
        public int? IncomeDetailingId { get; set; }
        public int DrugId { get; set; }
        public int ProducerId { get; set; }
        public DateTime ExpireDate { get; set; }
        public int DrugsCount { get; set; }
    }
}
