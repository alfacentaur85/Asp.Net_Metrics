using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using MetricsManager.Enums;
using MetricsManager;

namespace MetricsManagerTest
{
    public class AgentsControllerUnitTests
    {
        private AgentsController _controller;

        public AgentsControllerUnitTests()
        {
            _controller = new AgentsController();
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

