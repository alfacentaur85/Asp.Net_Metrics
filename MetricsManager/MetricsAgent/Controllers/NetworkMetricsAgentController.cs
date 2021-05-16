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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsAgentController : ControllerBase
    {
        private INetworkMetricsRepository _repository;

        private readonly ILogger<NetworkMetricsAgentController> _logger;

        public NetworkMetricsAgentController(INetworkMetricsRepository repository, ILogger<NetworkMetricsAgentController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "Nlog встроен в NetworkMetricsAgentController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            _repository.Create(new NetworkMetric
            {
                Time = request.Time,
                Value = request.Value

            });

            _logger.LogInformation(string.Concat("Create_Network: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            // задаем конфигурацию для мапера. Первый обобщенный параметр -- тип объекта источника, второй -- тип объекта в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkMetric, NetworkMetricDto>());
            var m = config.CreateMapper();
            IList<NetworkMetric> metrics = _repository.GetAll();
            var response = new NetworkAllMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // добавляем объекты в ответ при помощи мапера
                response.Metrics.Add(m.Map<NetworkMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetAll_Network"));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkMetric, NetworkMetricDto>());

            var m = config.CreateMapper();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new NetworkAllMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(m.Map<NetworkMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_Network: ", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
