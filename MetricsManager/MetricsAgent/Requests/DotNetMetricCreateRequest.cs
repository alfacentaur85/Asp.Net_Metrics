﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Requests
{
    public class DotNetMetricCreateRequest
    {
        
        public int Value { get; set; }

        public DateTimeOffset Time { get; set; }


    }
}
