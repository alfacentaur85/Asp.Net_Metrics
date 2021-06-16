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
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using AutoMapper;


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

        /// <summary>
        /// Сохраняет метрику Cpu на текущий момент времени
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            _repository.Create(new CpuMetric
            {
                Time = request.Time,
                Value = request.Value

            }) ;

            _logger.LogInformation(string.Concat("Create_CPU: ", " Time: ", request.Time.ToString(), " Value: ", request.Value.ToString()));

            return Ok();
        }

        /// <summary>
        /// Получает все метрики CPU
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET all
        ///
        /// </remarks>
        /// <returns>Список метрик CPU, которые были сохранены</returns>
        /// <response code="200">ОК</response>
        /// <response code="400">Неверные параметры</response> 
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            // задаем конфигурацию для мапера. Первый обобщенный параметр -- тип объекта источника, второй -- тип объекта в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CpuMetric, CpuMetricDto>());
            var m = config.CreateMapper();
            IList<CpuMetric> metrics = _repository.GetAll();
            var response = new CpuAllMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // добавляем объекты в ответ при помощи мапера
                response.Metrics.Add(m.Map<CpuMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetAll_CPU"));

            return Ok(response);
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Получает метрики CPU на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET from/1970-01-01/to/2021-12-31
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрка времени в секундах с 01.01.1970</param>
        /// <param name="toTime">конечная метрка времени в секундах с 01.01.1970</param>
        /// <returns>Список метрик CPU, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="200">ОК</response>
        /// <response code="400">Неверные параметры</response> 
>>>>>>> Lesson8
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CpuMetric, CpuMetricDto>());
            
            var m = config.CreateMapper();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new CpuAllMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(m.Map<CpuMetricDto>(metric));
            }

            _logger.LogInformation(string.Concat("GetMetricsFromAgent_CPU: "," fromTime: ", fromTime.ToString(), " toTime: ", toTime.ToString()));

            return Ok(response);
        }
    }
}
