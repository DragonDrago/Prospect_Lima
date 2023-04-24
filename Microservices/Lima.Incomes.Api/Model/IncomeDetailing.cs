using System;

namespace Lima.Incomes.Api.Model
{
    public class IncomeDetailing
    {
        public int IncomeDetailingId { get; set; }
        public Drug Drug { get; set; }
        public Producer Producer { get; set; }
        public DateTime ExpireDate { get; set; }
        public int DrugsCount { get; set; }
    }
}
