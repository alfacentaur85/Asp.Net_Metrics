using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
{
    public class Temperature
    {

        public DateTime dt { get; set; }
        public int vt { get; set; }

        public Temperature(DateTime dt, int vt)
        {
            this.dt = dt;
            this.vt = vt;
        }
    }
}
