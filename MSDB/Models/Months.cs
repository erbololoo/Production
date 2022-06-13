using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDB.Models
{
    public partial class Months
    {
        public Months()
        {
            Salaryes = new HashSet<Salaryes>();
        }
        public int Id { get; set; }
        public string Month_Name { get; set; }
        public virtual ICollection<Salaryes> Salaryes { get; set; }
    }
}
