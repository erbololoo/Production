using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class PurchaseOfRawMaterials
    {
        public int Id { get; set; }
        public int? RawMaterial { get; set; }
        public double? Amount { get; set; }
        public double? Sum { get; set; }
        public DateTime Date { get; set; }
        public int? Employee { get; set; }

        public virtual Employees EmployeeNavigation { get; set; }
        public virtual BuyRawMaterial RawMaterialNavigation { get; set; }
    }
}
