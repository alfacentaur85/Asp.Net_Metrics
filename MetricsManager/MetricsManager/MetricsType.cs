using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
{
    public enum MetricsTypeEnum
    {
        CpuMetrics,
        DotNetMetrics,
        HddMetrics,
        NetworkMetrics,
        RamMetrics,
        Agents
    }
    public class MetricsType
    {
        public static List<string> metricsList = new List<string> { "cpumetrics", "dotnetmetrics", "hddmetrics", "networkmetrics", "rammetrics", "agents" };
    }
}
