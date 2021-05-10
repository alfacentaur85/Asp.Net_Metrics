using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    enum MetricsTypeEnum
    {
        CPU_METRICS,
        DOTNET_METRICS,
        HDD_METRICS,
        NETWORK_METRICS,
        RAM_METRICS
    }
    public class MetricsType
    {
        public static List<string> metricsList = new List<string> { "cpumetrics", "dotnetmetrics", "hddmetrics", "networkmetrics", "rammetrics" };
    }
}
