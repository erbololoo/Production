using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class Budget
    {
        public int Id { get; set; }
        public float? SumBudget { get; set; }
        public decimal? Procent { get; set; }
        public decimal? Bonus { get; set; }
    }
}
