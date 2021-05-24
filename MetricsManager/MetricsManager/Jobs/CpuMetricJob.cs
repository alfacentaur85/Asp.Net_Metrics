using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using MetricsManager.Client;
using MetricsManager.Requests;
using MetricsManager.Responses;


namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;

        private IAgentsRepositorySingle _repositoryAgents;

        private IMetricsAgentClient _metricsAgentClient;

        public CpuMetricJob(ICpuMetricsRepository repository, IMetricsAgentClient metricsAgentClient, IAgentsRepositorySingle repositoryAgents)
        {
            _repository = repository;

            _metricsAgentClient = metricsAgentClient;

            _repositoryAgents = repositoryAgents;
        }

        public Task Execute(IJobExecutionContext context)
        {

            var agentsList = _repositoryAgents.GetAll();

            foreach(var agent in agentsList)
            {
                var metricsList = _metricsAgentClient.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest
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

