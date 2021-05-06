using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsAgent.Requests;

namespace MetricsAgent.Controllers
{
    [Route("api/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsAgentController : ControllerBase
    {
        private IDotNetMetricsRepository _repository;

        private readonly ILogger<DotNetMetricsAgentController> _logger;

        public DotNetMetricsAgentController(IDotNetMetricsRepository repository, ILogger<DotNetMetricsAgentController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "Nlog встроен в DotNetMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MetricCreateRequest request)
        {
            _repository.Create(new Metric
            {
                Time = request.Time,
                Value = request.Value

            });

            _logger.LogInformation(string.Concat("Create_DotNet: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll() ?? new List<Metric>();

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new MetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            _logger.LogInformation(string.Concat("GetAll_DotNet"));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime) ?? new List<Metric>();

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new MetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_DotNet: ", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
