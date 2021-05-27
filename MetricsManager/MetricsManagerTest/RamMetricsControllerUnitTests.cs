using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTest
{
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;

        private readonly ILogger<RamMetricsController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<RamMetricsController>();

        private readonly MetricsManager.DAL.Interfaces.IRamMetricsRepository _repository = new MetricsManager.DAL.Repositories.RamMetricsRepository();

        public RamControllerUnitTests()
        {
            _controller = new RamMetricsController(_logger, _repository);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOK()
        {
            //Arrange
            var agentId = 1;
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);

            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOK()
        {
            //Arrange
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);

            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}
