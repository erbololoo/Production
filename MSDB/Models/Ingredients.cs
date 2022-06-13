using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class Ingredients
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public int? RawMaterial { get; set; }
        public double? Amount { get; set; }

        public virtual FinishedProducts ProductNavigation { get; set; }
        public virtual BuyRawMaterial RawMaterialNavigation { get; set; }
    }
}
