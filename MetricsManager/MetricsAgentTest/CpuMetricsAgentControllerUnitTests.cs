using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MetricsAgent.Responses;
using NLog.Web;
using System.Collections.Generic;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;

namespace MetricsAgentTest
{
    public class CpuControllerUnitTests
    {
        private CpuMetricsAgentController _controller;

        private Mock<ICpuMetricsRepository> _mock;

        private readonly ILogger<CpuMetricsAgentController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<CpuMetricsAgentController>();

        public CpuControllerUnitTests()
        {
            _mock = new Mock<ICpuMetricsRepository>();

            _controller = new CpuMetricsAgentController(_mock.Object, _logger);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит Metric объект
            _mock.Setup(repository =>
            repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            // выполняем действие на контроллере
            var result = _controller.Create(new
            MetricsAgent.Requests.CpuMetricCreateRequest
            {
                Time = DateTimeOffset.FromUnixTimeSeconds(1),
                Value = 50
            });

            _mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());

        }

        [Fact]
        public void All_ShouldCall_GetAll_From_Repository()
        {
 
            _mock.Setup(repository =>
            repository.GetAll());
            // выполняем действие на контроллере
            var result = _controller.GetAll();

            _mock.Verify(repository => repository.GetAll());
        }

        [Fact]
        public void ByPeriod_ShouldCall_GetMetricsFromAgent_From_Repository()
        {

            _mock.Setup(repository =>
            repository.GetByPeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()));
            // выполняем действие на контроллере
            var result = _controller.GetMetricsFromAgent(DateTimeOffset.FromUnixTimeSeconds(1), DateTimeOffset.FromUnixTimeSeconds(100));

            _mock.Verify(repository => repository.GetByPeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()));

        }
    }
}

