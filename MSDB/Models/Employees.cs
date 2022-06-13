using System;
using System.Collections.Generic;
using MSDB.Models;

namespace MSDB
{
    public partial class Employees
    {
        public Employees()
        {
            Prodution = new HashSet<Prodution>();
            PurchaseOfRawMaterials = new HashSet<PurchaseOfRawMaterials>();
            SaleOfProducts = new HashSet<SaleOfProducts>();
            Salari = new HashSet<Salaryes>();
        }

        public int Id { get; set; }
        public string Fio { get; set; }
        public int? Position { get; set; }
        public double? Salary { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Positions PositionNavigation { get; set; }
        public virtual ICollection<Prodution> Prodution { get; set; }
        public virtual ICollection<PurchaseOfRawMaterials> PurchaseOfRawMaterials { get; set; }
        public virtual ICollection<Salaryes> Salari { get; set; }
        public virtual ICollection<SaleOfProducts> SaleOfProducts { get; set; }
    }
}
