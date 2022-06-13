using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class FinishedProducts
    {
        public FinishedProducts()
        {
            Ingredients = new HashSet<Ingredients>();
            Prodution = new HashSet<Prodution>();
            SaleOfProducts = new HashSet<SaleOfProducts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Unites { get; set; }
        public double? Sum { get; set; }
        public double? Amount { get; set; }

        public virtual Units UnitNavigation { get; set; }
        public virtual ICollection<Ingredients> Ingredients { get; set; }
        public virtual ICollection<Prodution> Prodution { get; set; }
        public virtual ICollection<SaleOfProducts> SaleOfProducts { get; set; }
    }
}
