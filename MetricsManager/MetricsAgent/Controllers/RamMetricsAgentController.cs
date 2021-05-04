﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsAgent.Requests;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram/available")]
    [ApiController]
    public class RamMetricsAgentController : ControllerBase
    {
        private IRamMetricsRepository _repository;

        private readonly ILogger<RamMetricsAgentController> _logger;

        public RamMetricsAgentController(IRamMetricsRepository repository, ILogger<RamMetricsAgentController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "Nlog встроен в RamMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MetricCreateRequest request)
        {
            _repository.Create(new Metric
            {
                Time = request.Time,
                Value = request.Value

            });

            _logger.LogInformation(string.Concat("Create_Ram: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll() ?? new List<Metric>();

            var response = new AllMetricsResponse()
            {
                Metrics = new List<Metric>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new Metric { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            _logger.LogInformation(string.Concat("GetAll_Ram"));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime) ?? new List<Metric>();

            var response = new AllMetricsResponse()
            {
                Metrics = new List<Metric>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new Metric { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_Ram: ", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
