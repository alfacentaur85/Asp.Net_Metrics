using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public enum MetricsTypeEnum
    {
        CpuMetrics,
        DotNetMetrics,
        HddMetrics,
        NetworkMetrics,
        RamMetrics
    }
    public class MetricsType
    {
        public static List<string> metricsList = new List<string> { "cpumetrics", "dotnetmetrics", "hddmetrics", "networkmetrics", "rammetrics" };
    }
}
