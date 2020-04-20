using AutoMapper;
using DeliveryManager.Models;
using DeliveryManager.Repositories.Contracts;
using DeliveryManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryManager.Tests
{
    [TestClass]
    public class DeliveryWindowServiceTests
    {
        [TestMethod]
        [DataRow(DeliveryWindowType.Regular, null, null)]
        [DataRow(DeliveryWindowType.Urgent, 8, 19)]
        public void AddWindow(DeliveryWindowType type, int start, int finish)
        {
            // Given
            var id = Guid.NewGuid();
            var currentDate = new DateTime(2020, 01, 15, 09, 0, 0);

            var request = RequestsFactory.CreateWindowRequest(type: type, start: new TimeSpan(start, 0, 0),
                finish: new TimeSpan(finish, 0, 0));
            var window = ModelsFactory.DeliveryWindow(id, request.Name, request.Description, request.Price,
                request.Type, request.ExpectedDeliveryTimeStart, request.ExpectedDeliveryTimeFinish,
                request.AvailableFrom, request.AvailableTo, request.AvailableDays, request.AvailabilityTimeLimit);

            var repo = new Mock<IDeliveryWindowRepo>();
            repo.Setup(x => x.Add(window)).Returns(id);
            repo.Setup(x => x.Get(currentDate)).Returns(new List<DeliveryWindow> { window });

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<DeliveryWindow>(request)).Returns(window);

            var service = new DeliveryWindowService(repo.Object, mapper.Object);

            // When
            var actualId = service.Add(request);

            // Then
            var actual = service.Get(currentDate, 1);

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.GroupBy(x => x.Type).Count() == 1);
            Assert.IsTrue(actual.GroupBy(x => x.Type).First().Key == type);

            CollectionAssert.Contains(actual.ToList(),
                actual.FirstOrDefault(x => x.Start == currentDate.Date.Add(window.ExpectedDeliveryTimeStart)));
        }

        [TestMethod]
        public void UpdateWindow()
        {
            // Given
            var id = Guid.NewGuid();
            var currentDate = new DateTime(2020, 01, 15, 09, 0, 0);

            var createRequest = RequestsFactory.CreateWindowRequest();
            var window = ModelsFactory.DeliveryWindow(id, createRequest.Name, createRequest.Description, createRequest.Price,
                createRequest.Type, createRequest.ExpectedDeliveryTimeStart, createRequest.ExpectedDeliveryTimeFinish,
                createRequest.AvailableFrom, createRequest.AvailableTo, createRequest.AvailableDays,
                createRequest.AvailabilityTimeLimit);

            var updateRequest = RequestsFactory.UpdateWindowRequest(name: "new_name", description: "new_description",
                price: 99, start: new TimeSpan(10, 0, 0), finish: new TimeSpan(14, 0, 0),
                timeLimit: new TimeSpan(02, 0, 0));
            var updatedWindow = ModelsFactory.DeliveryWindow(id, updateRequest.Name, updateRequest.Description,
                updateRequest.Price, createRequest.Type, updateRequest.ExpectedDeliveryTimeStart,
                updateRequest.ExpectedDeliveryTimeFinish, createRequest.AvailableFrom, createRequest.AvailableTo,
                createRequest.AvailableDays, updateRequest.AvailabilityTimeLimit);

            var dto = ModelsFactory.DeliveryWindowDto(createRequest.Name, createRequest.Description, createRequest.Price,
                createRequest.Type, currentDate.Date.Add(createRequest.ExpectedDeliveryTimeStart),
                currentDate.Date.Add(createRequest.ExpectedDeliveryTimeFinish), false);

            var repo = new Mock<IDeliveryWindowRepo>();
            repo.Setup(x => x.Add(window)).Returns(id);
            repo.Setup(x => x.Update(id, window)).Returns(updatedWindow);
            repo.Setup(x => x.Get(currentDate)).Returns(new List<DeliveryWindow> { updatedWindow });

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<DeliveryWindow>(createRequest)).Returns(window);
            mapper.Setup(x => x.Map<DeliveryWindow>(updateRequest)).Returns(updatedWindow);
            mapper.Setup(x => x.Map<DeliveryWindowDto>(updatedWindow)).Returns(dto);

            var service = new DeliveryWindowService(repo.Object, mapper.Object);
            var addedItemId = service.Add(createRequest);

            // When
            var updated = service.Update(addedItemId, updateRequest);

            // Then
            var e = service.Get(currentDate, 1);
            var actual = e.FirstOrDefault(x => x.Name == updateRequest.Name && !x.Available);

            Assert.IsNotNull(actual);
            Assert.AreEqual(updateRequest.Name, actual.Name);
            Assert.AreEqual(updateRequest.Description, actual.Description);
            Assert.AreEqual(updateRequest.Price, actual.Price);
            Assert.AreEqual(updateRequest.Type, actual.Type);
            Assert.AreEqual(currentDate.Date.Add(updateRequest.ExpectedDeliveryTimeStart), actual.Start);
            Assert.AreEqual(currentDate.Date.Add(updateRequest.ExpectedDeliveryTimeFinish), actual.Finish);
        }

        [TestMethod]
        public void Get_AvailabilityTimeoutExpired()
        {
            // Given
            var id = Guid.NewGuid();
            var currentDate = new DateTime(2020, 01, 15, 12, 0, 0);

            var request = RequestsFactory.CreateWindowRequest();
            var window = ModelsFactory.DeliveryWindow(id, request.Name, request.Description, request.Price,
                request.Type, request.ExpectedDeliveryTimeStart, request.ExpectedDeliveryTimeFinish,
                request.AvailableFrom, request.AvailableTo, request.AvailableDays, request.AvailabilityTimeLimit);

            var repo = new Mock<IDeliveryWindowRepo>();
            repo.Setup(x => x.Add(window)).Returns(id);
            repo.Setup(x => x.Get(currentDate)).Returns(new List<DeliveryWindow> { window });

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<DeliveryWindow>(request)).Returns(window);

            var service = new DeliveryWindowService(repo.Object, mapper.Object);

            var actualId = service.Add(request);

            // When
            var actual = service.Get(currentDate, 1);

            // Then
            var doesNotAvailableDto = actual.First(x => x.Available == false);

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.GroupBy(x => x.Type).Count() == 1);
            Assert.IsTrue(actual.GroupBy(x => x.Type).First().Key == DeliveryWindowType.Regular);
            Assert.IsFalse((doesNotAvailableDto.Start.TimeOfDay.TotalMilliseconds - currentDate.TimeOfDay.TotalMilliseconds) >=
                window.AvailabilityTimeLimit.TotalMilliseconds);

            CollectionAssert.Contains(actual.ToList(),
                actual.FirstOrDefault(x => x.Start == currentDate.Date.Add(window.ExpectedDeliveryTimeStart)));
        }
    }
}
