using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Interfaces;
using System.Diagnostics;
using MetricsAgent.DAL.Models;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;

        // счетчик для метрики Network
        private PerformanceCounter _NetworkCounter;
        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            _NetworkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", performanceCounterCategory.GetInstanceNames()[0]);
        }

        public Task Execute(IJobExecutionContext context)
        {

            var networkByteReceivedBySec = Convert.ToInt32(_NetworkCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new NetworkMetric { Time = time, Value = networkByteReceivedBySec });

            return Task.CompletedTask;
        }

    }
}
