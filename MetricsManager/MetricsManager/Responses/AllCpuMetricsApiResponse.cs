using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Models;
using MetricsManager.DTO;

namespace MetricsManager.Responses
{
    public class AllCpuMetricsApiResponse
    {
        public List<CpuMetric> Metrics { get; set; }
        
    }
}
