using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Interfaces;
using System.Diagnostics;
using MetricsAgent.DAL.Models;
using System.IO;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;

        // счетчик для метрики DotNet

        private PerformanceCounter _DotNetCounter;
        public DotNetMetricJob(IDotNetMetricsRepository repository)
        {
            
            _repository = repository;

            _DotNetCounter = new PerformanceCounter(".Net CLR Memory", "Gen 0 Heap Size", "iisexpresstray");
        }

        public Task Execute(IJobExecutionContext context)
        {
   
            var dotNet = Convert.ToInt32(_DotNetCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow;

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DotNetMetric { Time = time, Value = dotNet });

            return Task.CompletedTask;
        }
    }
}
