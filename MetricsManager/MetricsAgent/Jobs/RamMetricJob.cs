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
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;

        // счетчик для метрики Ram

        private PerformanceCounter _RamCounter;

        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _RamCounter = new PerformanceCounter("Memory", "Available MBytes", null);
        }

        public Task Execute(IJobExecutionContext context)
        {

            var ramMemoryAvailable = Convert.ToInt32(_RamCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new RamMetric { Time = time, Value = ramMemoryAvailable });

            return Task.CompletedTask;
        }

    }
}
