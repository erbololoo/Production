using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class SaleOfProducts
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public double? Amount { get; set; }
        public double? Sum { get; set; }
        public DateTime Date { get; set; }
        public int? Employee { get; set; }

        public virtual Employees EmployeeNavigation { get; set; }
        public virtual FinishedProducts ProductNavigation { get; set; }
    }
}
