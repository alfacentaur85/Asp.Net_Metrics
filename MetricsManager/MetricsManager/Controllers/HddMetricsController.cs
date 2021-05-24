using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Responses;
using MetricsManager.DAL.Models;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository _repository;

        private readonly ILogger<HddMetricsController> _logger;
        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "NLog втроен в HddMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var response = _repository.GetByAgentByPeriod(agentId, fromTime, toTime);

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_HDD: ", " AgentId: ", agentId.ToString(), " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var response = _repository.GetByPeriod(fromTime, toTime);

            _logger.LogInformation("GetMetricsFromAllCluster_HDD:", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString());

            return Ok(response);
        }
    }
}
