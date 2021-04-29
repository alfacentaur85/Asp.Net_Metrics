using MetricsManager.Controllers;
using System;
using Xunit;

namespace MetricsManagerTest
{
    public class CpuControllerUnitTests
    {
        private CpuMetricsController _controller;

        public CpuControllerUnitTests()
        {
            _controller = new CpuMetricsController();
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOK()
        {
            //Arrange
            var agentOd = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = _controller.Get
        }
    }
}
