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
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;

        // счетчик для метрики Hdd

        private PerformanceCounter _HddCounter;

        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _HddCounter = new PerformanceCounter("LogicalDisk", "% Free Space", "C:");
        }

        public Task Execute(IJobExecutionContext context)
        {
            
            var hddFreeInPercents = Convert.ToInt32(_HddCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new HddMetric { Time = time, Value = hddFreeInPercents });

            return Task.CompletedTask;
        }

    }
}
