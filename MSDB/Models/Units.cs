using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class Units
    {
        public Units()
        {
            BuyRawMaterial = new HashSet<BuyRawMaterial>();
            FinishedProducts = new HashSet<FinishedProducts>();
        }

        public int Id { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<BuyRawMaterial> BuyRawMaterial { get; set; }
        public virtual ICollection<FinishedProducts> FinishedProducts { get; set; }
    }
}
