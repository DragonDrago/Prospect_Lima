using System;
using System.Collections.Generic;

namespace Lima.Incomes.Api.Requests
{
    public class IncomeRequest
    {
        public string IncomeName { get; set; } = $"Новый приход за {DateTime.Now.ToString("d")}";  
        public IEnumerable<IncomeDetailingRequest> IncomeDetailings { get; set; }
    }
}
