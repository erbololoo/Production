using System;
using System.Collections.Generic;

namespace MSDB
{
    public partial class GotovayaProduction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double? Sum { get; set; }
        public double? Amount { get; set; }
    }
}
