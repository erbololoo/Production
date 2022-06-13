using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class BuyRawMaterial
    {
        public BuyRawMaterial()
        {
            Ingredients = new HashSet<Ingredients>();
            PurchaseOfRawMaterials = new HashSet<PurchaseOfRawMaterials>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Unites { get; set; }
        public double? Sum { get; set; }
        public double? Amount { get; set; }

        public virtual Units UnitNavigation { get; set; }
        public virtual ICollection<Ingredients> Ingredients { get; set; }
        public virtual ICollection<PurchaseOfRawMaterials> PurchaseOfRawMaterials { get; set; }
    }
}
