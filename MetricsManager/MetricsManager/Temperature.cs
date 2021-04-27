using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
{
    public class Temperature
    {

        public List<(DateTime, int)> listTimeTemp;

        public Temperature()
        {
            listTimeTemp = new List<(DateTime, int)>();       
        }
    }
}
