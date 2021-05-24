using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Models;

namespace MetricsManager.Responses
{
    public class AllDotNetMetricsApiResponse
    {
        public List<DotNetMetric> Metrics { get; set; }
    }
}
