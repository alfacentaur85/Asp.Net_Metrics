using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Models;
using Core.Interfaces;

namespace MetricsAgent.DAL.Interfaces
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {

    }
}
