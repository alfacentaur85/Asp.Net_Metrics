using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsManager.DAL.Interfaces;
using System.Diagnostics;
using MetricsManager.DAL.Models;
using MetricsManager.Requests;

namespace MetricsManager.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;

        private IAgentsRepositorySingle _repositoryAgents;

        private IMetricsAgentClient _metricsAgentClient;


        public RamMetricJob(IRamMetricsRepository repository, IMetricsAgentClient metricsAgentClient, IAgentsRepositorySingle repositoryAgents)
        {
            _repository = repository;

            _metricsAgentClient = metricsAgentClient;

            _repositoryAgents = repositoryAgents;
        }
        public Task Execute(IJobExecutionContext context)
        {

            var agentsList = _repositoryAgents.GetAll();

            foreach (var agent in agentsList)
            {
                var metricsList = _metricsAgentClient.GetAllRamMetrics(new GetAllRamMetricsApiRequest
                {
                    FromTime = _repository.GetMaxDate(agent.AgentId),
                    ToTime = DateTimeOffset.UtcNow,
                    ClientBaseAddress = agent.AgentURL
                });

                if (!object.ReferenceEquals(metricsList, null))
                {
                    foreach (var metric in metricsList.Metrics)
                    {
                        _repository.Create(metric, agent.AgentId);
                    }
                }
            }

            return Task.CompletedTask;
        }

    }
}
