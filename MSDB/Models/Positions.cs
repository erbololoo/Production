using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class Positions
    {
        public Positions()
        {
            Employees = new HashSet<Employees>();
        }

        public int Id { get; set; }
        public string Position { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
