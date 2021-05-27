using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MetricsAgent.Responses;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;

namespace MetricsAgentTest
{
    public class RamControllerUnitTests
    {
        private RamMetricsAgentController _controller;

        private Mock<IRamMetricsRepository> _mock;

        private readonly ILogger<RamMetricsAgentController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<RamMetricsAgentController>();

        public RamControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();

            _controller = new RamMetricsAgentController(_mock.Object, _logger);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит Metric объект
            _mock.Setup(repository =>
            repository.Create(It.IsAny<RamMetric>())).Verifiable();
            // выполняем действие на контроллере
            var result = _controller.Create(new
            MetricsAgent.Requests.RamMetricCreateRequest
            {
                Time = DateTimeOffset.FromUnixTimeSeconds(1),
                Value = 50
            });

        }

        [Fact]
        public void All_ShouldCall_GetAll_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит Metric объект
            _mock.Setup(repository =>
            repository.GetAll()).Verifiable();
            // выполняем действие на контроллере
            var result = _controller.GetAll();

        }

        [Fact]
        public void ByPeriod_ShouldCall_GetMetricsFromAgent_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит Metric объект
            _mock.Setup(repository =>
            repository.GetById(It.IsAny<int>()));
            // выполняем действие на контроллере
            var result = _controller.GetMetricsFromAgent(DateTimeOffset.FromUnixTimeSeconds(1), DateTimeOffset.FromUnixTimeSeconds(100));

        }
    }
}

