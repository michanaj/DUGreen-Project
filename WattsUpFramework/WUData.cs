using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WattsUpFramework
{
    public class WUData
    {
        public string Record { get; set; }
        public DateTime Time { get; set; }
        public double Watts { get; set; }
        public string WattMax { get; set; }
        public string WattMin { get; set; }
        public string Volts { get; set; }
        public string Amps { get; set; }
        public string WattHours { get; set; }
        public string Cost { get; set; }
        public string WattHoursPerMonth { get; set; }
        public string CostPerMonth { get; set; }

    }
}
