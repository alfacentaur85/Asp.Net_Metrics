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
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsAgentController _controller;

        private Mock<IDotNetMetricsRepository> _mock;

        private readonly ILogger<DotNetMetricsAgentController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<DotNetMetricsAgentController>();

        public DotNetControllerUnitTests()
        {
            _mock = new Mock<IDotNetMetricsRepository>();

            _controller = new DotNetMetricsAgentController(_mock.Object, _logger);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит Metric объект
            _mock.Setup(repository =>
            repository.Create(It.IsAny<DotNetMetric>())).Verifiable();
            // выполняем действие на контроллере
            var result = _controller.Create(new
            MetricsAgent.Requests.DotNetMetricCreateRequest
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
            repository.GetAll());
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

