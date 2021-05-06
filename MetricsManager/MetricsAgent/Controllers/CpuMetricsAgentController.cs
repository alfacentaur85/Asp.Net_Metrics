using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsAgent.Requests;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsAgentController : ControllerBase
    {
        private ICpuMetricsRepository _repository;

        private readonly ILogger<CpuMetricsAgentController> _logger;

        public CpuMetricsAgentController(ICpuMetricsRepository repository, ILogger<CpuMetricsAgentController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "Nlog встроен в CpuMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MetricCreateRequest request)
        {
            _repository.Create(new Metric
            {
                Time = request.Time,
                Value = request.Value

            }) ;

            _logger.LogInformation(string.Concat("Create_CPU: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

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

            foreach(var metric in metrics)
            {
                response.Metrics.Add(new MetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id }); 
            }

            _logger.LogInformation(string.Concat("GetAll_CPU"));

            /*return Ok(response);*/
            return Ok(metrics[0].Time);
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

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_CPU: "," fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }

    }
}
