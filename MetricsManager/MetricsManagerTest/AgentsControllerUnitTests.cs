using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using MetricsManager;
using Microsoft.Extensions.Logging;
using NLog.Web;


namespace MetricsManagerTest
{




    public class AgentsControllerUnitTests
    {
        private AgentsController _controller;

        private readonly ILogger<AgentsController> _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<AgentsController>();

        public AgentsControllerUnitTests()
        {
            _controller = new AgentsController(_logger);
        }

        [Fact]
        public void RegisterAgent_ReturnsOK()
        {
            //Arrange
            var agentInfo = new AgentInfo();


            //Act
            var result = _controller.RegisterAgent(agentInfo);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }


        [Fact]
        public void EnableAgentById_ReturnsOK()
        {
            //Arrange
            var agentId = 1;


            //Act
            var result = _controller.EnableAgentById(agentId);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOK()
        {
            //Arrange
            var agentId = 1;


            //Act
            var result = _controller.DisableAgentById(agentId);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}

