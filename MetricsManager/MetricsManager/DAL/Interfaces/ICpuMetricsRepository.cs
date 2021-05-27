using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL;
using MetricsManager.DAL.Models;
using Core.Interfaces;
using MetricsManager.Responses;
using MetricsManager.DTO;

namespace MetricsManager.DAL.Interfaces
{
    public interface ICpuMetricsRepository : IRepositoryManager<CpuMetric>
    {

    }
}
