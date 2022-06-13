using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSDB.Models
{
    public partial class Years
    {
        public Years()
        {
            Salari = new HashSet<Salaryes>();
        }
        [Key]
        public int Year_Name { get; set; }
        public virtual ICollection<Salaryes> Salari { get; set; }
    }
}
