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
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
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

        /// <summary>
        /// Сохраняет метрику DotNet на текущий момент времени
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _repository.Create(new DotNetMetric
            {
                Time = request.Time,
                Value = request.Value

            });

            _logger.LogInformation(string.Concat("Create_DotNet: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

            return Ok();
        }

        /// <summary>
        /// Получает все метрики DotNet
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET all
        ///
        /// </remarks>
        /// <returns>Список метрик DotNet, которые были сохранены</returns>
        /// <response code="200">ОК</response>
        /// <response code="400">Неверные параметры</response> 
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            // задаем конфигурацию для мапера. Первый обобщенный параметр -- тип объекта источника, второй -- тип объекта в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetric, DotNetMetricDto>());
            var m = config.CreateMapper();
            IList<DotNetMetric> metrics = _repository.GetAll(); 
            var response = new DotNetAllMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // добавляем объекты в ответ при помощи мапера
                response.Metrics.Add(m.Map<DotNetMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetAll_DotNet"));

            return Ok(response);
        }

        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET from/1970-01-01/to/2021-12-31
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрка времени в секундах с 01.01.1970</param>
        /// <param name="toTime">конечная метрка времени в секундах с 01.01.1970</param>
        /// <returns>Список метрик DotNet, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">ОК</response>
        /// <response code="400">Неверные параметры</response> 
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetric, DotNetMetricDto>());

            var m = config.CreateMapper();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new DotNetAllMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(m.Map<DotNetMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_DotNet: ", " fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
