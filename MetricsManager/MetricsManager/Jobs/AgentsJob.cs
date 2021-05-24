using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;



namespace MetricsManager.Jobs
{
    public class AgentsJob : IJob
    {
        private IAgentsRepositorySingle _repository;
        public AgentsJob(IAgentsRepositorySingle repository)
        {
            _repository = repository;


        }
        public Task Execute (IJobExecutionContext context)
        {

           _repository.Create(new Agents {AgentId = 1, AgentURL = "http://localhost:5000" });

            return Task.CompletedTask;
        }

        public Task Delete()
        {

            _repository.Delete();

            return Task.CompletedTask;
        }
    }
}

