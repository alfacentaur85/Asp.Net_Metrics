﻿using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTest
{
    public class HddControllerUnitTests
    {
        private HddMetricsController _controller;

        private readonly ILogger<HddMetricsController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<HddMetricsController>();

        private readonly MetricsManager.DAL.Interfaces.IHddMetricsRepository _repository = new MetricsManager.DAL.Repositories.HddMetricsRepository();

        public HddControllerUnitTests()
        {
            _controller = new HddMetricsController(_logger, _repository);
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
