using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Responses;

namespace MetricsAgent.Responses
{
    public class AllMetricsResponse
    {
        public List<Metric> Metrics { get; set; }
    }
}
