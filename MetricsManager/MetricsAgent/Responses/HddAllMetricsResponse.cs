using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Responses;

namespace MetricsAgent.Responses
{
    public class HddAllMetricsResponse
    {
        public List<HddMetricDto> Metrics { get; set; }
    }

    public class HddMetricDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time {get; set;}
       
        
    }

}
