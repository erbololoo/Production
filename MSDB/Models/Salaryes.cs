using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDB.Models
{
    public partial class Salaryes
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int? Employee { get; set; }
        public short ParticipationInThePurchase { get; set; }
        public short ParticipationOnSale { get; set; }
        public short ParticipationOnProduction { get; set; }
        public int TotalNumberOfParticipations { get; set; }
        public decimal Bonus { get; set; }
        public float Salary { get; set; }
        public float TotalSum { get; set; }
        public bool Boolean { get; set; }

        public virtual Employees EmployeeNavigation { get; set; }
        public virtual Years Year_NameNavigation { get; set; }
        public virtual Months MonthNavigation { get; set; }
    }
}
