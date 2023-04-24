using System;
using System.Collections.Generic;

namespace Lima.Incomes.Api.Model
{
    public class Income
    {
        public int Id { get; set; }
        public string IncomeName { get; set; }
        public DateTime DateCreate { get; set; }
        public IEnumerable<IncomeDetailing> IncomeDetailings { get; set; }
    }
}
