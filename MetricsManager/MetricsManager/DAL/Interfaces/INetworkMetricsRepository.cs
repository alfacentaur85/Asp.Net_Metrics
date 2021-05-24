using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Models;
using Core.Interfaces;

namespace MetricsManager.DAL.Interfaces
{
    public interface INetworkMetricsRepository : IRepositoryManager<NetworkMetric>
    {

    }
}
