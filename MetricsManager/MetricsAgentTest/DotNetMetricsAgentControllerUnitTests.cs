using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTest
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsAgentController _controller;

        public DotNetControllerUnitTests()
        {
            _controller = new DotNetMetricsAgentController();
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOK()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}

