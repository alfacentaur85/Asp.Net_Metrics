using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTest
{
    public class RamControllerUnitTests
    {
        private RamMetricsAgentController _controller;

        public RamControllerUnitTests()
        {
            _controller = new RamMetricsAgentController();
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
