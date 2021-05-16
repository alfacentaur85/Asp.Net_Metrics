using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsAgent.Requests;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
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
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _repository.Create(new RamMetric
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
            // задаем конфигурацию для мапера. Первый обобщенный параметр -- тип объекта источника, второй -- тип объекта в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetric, RamMetricDto>());
            var m = config.CreateMapper();
            IList<RamMetric> metrics = _repository.GetAll();
            var response = new RamAllMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // добавляем объекты в ответ при помощи мапера
                response.Metrics.Add(m.Map<RamMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetAll_Ram"));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetric, RamMetricDto>());

            var m = config.CreateMapper();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new RamAllMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(m.Map<RamMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_Ram: ", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
