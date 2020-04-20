using DeliveryManager.Controllers;
using DeliveryManager.Models.Requests;
using DeliveryManager.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DeliveryManager.Tests
{
    [TestClass]
    public class DeliveryWindowControllerTests
    {
        private Mock<IDeliveryWindowService> _service;
        private DeliveryWindowController _controller;

        [TestInitialize]
        public void OnSetUp()
        {
            _service = new Mock<IDeliveryWindowService>();
            _controller = new DeliveryWindowController(_service.Object);
        }

        [TestMethod]
        public void Get_NegativeHorizon_BadRequest()
        {
            var response = _controller.Get(DateTime.MinValue, -1) as BadRequestObjectResult;
            Assert.AreEqual(400, response.StatusCode);
            Assert.AreEqual("The horizon parameter cannot be less than 0", response.Value.ToString());
        }

        [TestMethod]
        public void Get_HorizonTooBig_BadRequest()
        {
            var response = _controller.Get(DateTime.MinValue, 99) as BadRequestObjectResult;
            Assert.AreEqual(400, response.StatusCode);
            Assert.AreEqual("The horizon parameter cannot be more than 50", response.Value.ToString());
        }

        [TestMethod]
        public void Update_IdIsNull_BadRequest()
        {
            var response = _controller.Update(Guid.Empty, new UpdateWindowRequest()) as BadRequestObjectResult;
            Assert.AreEqual(400, response.StatusCode);
            Assert.AreEqual("The id parameter should be defined", response.Value.ToString());
        }
    }
}
