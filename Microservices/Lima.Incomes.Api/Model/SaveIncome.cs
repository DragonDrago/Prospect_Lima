using System.Collections.Generic;

namespace Lima.Incomes.Api.Model
{
    public class SaveIncome
    {
        public int Id { get; set; }
        public string IncomeName { get; set; }
        public IEnumerable<SaveIncomeDetailing> IncomeDetailings { get; set; }
    }
}
