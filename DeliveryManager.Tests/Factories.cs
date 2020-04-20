using DeliveryManager.Models;
using DeliveryManager.Models.Requests;
using System;

namespace DeliveryManager.Tests
{
    public class RequestsFactory
    {
        public static CreateWindowRequest CreateWindowRequest(string name = null, string description = null,
            decimal? price = null, DeliveryWindowType? type = null, TimeSpan? start = null, TimeSpan? finish = null,
            DateTime? from = null, DateTime? to = null, TimeSpan? timeLimit = null) =>
            new CreateWindowRequest
        {
            Name = name ?? "name",
            Description = description ?? "description",
            Price = price ?? new decimal(10),
            Type = type ?? DeliveryWindowType.Regular,
            ExpectedDeliveryTimeStart = start ?? new TimeSpan(14, 0, 0),
            ExpectedDeliveryTimeFinish = finish ?? new TimeSpan(19, 0, 0),
            AvailableFrom = from ?? new DateTime(2020, 01, 01),
            AvailableTo = to ?? new DateTime(2020, 02, 01),
            AvailabilityTimeLimit = timeLimit ?? new TimeSpan(04, 0, 0)
        };

        public static UpdateWindowRequest UpdateWindowRequest(string name = null, string description = null,
            decimal? price = null, DeliveryWindowType? type = null, TimeSpan? start = null, TimeSpan? finish = null,
            DateTime? from = null, DateTime? to = null, TimeSpan? timeLimit = null) =>
            new UpdateWindowRequest
            {
                Name = name ?? "name",
                Description = description ?? "description",
                Price = price ?? new decimal(10),
                Type = type ?? DeliveryWindowType.Regular,
                ExpectedDeliveryTimeStart = start ?? new TimeSpan(14, 0, 0),
                ExpectedDeliveryTimeFinish = finish ?? new TimeSpan(19, 0, 0),
                AvailableFrom = from ?? new DateTime(2020, 01, 01),
                AvailableTo = to ?? new DateTime(2020, 02, 01),
                AvailabilityTimeLimit = timeLimit ?? new TimeSpan(04, 0, 0)
            };
    }

    public class ModelsFactory
    {
        public static DeliveryWindow DeliveryWindow(Guid? id = null, string name = null, string description = null,
            decimal? price = null, DeliveryWindowType? type = null, TimeSpan? start = null, TimeSpan? finish = null,
            DateTime? from = null, DateTime? to = null, string availableDays = null, TimeSpan? timeLimit = null) =>
            new DeliveryWindow
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "name",
            Description = description ?? "description",
            Price = price ?? new decimal(49),
            Type = type ?? DeliveryWindowType.Regular,
            ExpectedDeliveryTimeStart = start ?? new TimeSpan(14, 00, 00),
            ExpectedDeliveryTimeFinish = finish ?? new TimeSpan(14, 00, 00),
            AvailableFrom = from ?? DateTime.Now,
            AvailableTo = to ?? DateTime.Now.AddDays(33),
            AvailableDays = availableDays ?? "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday",
            AvailabilityTimeLimit = timeLimit ?? new TimeSpan(04, 0, 0)
        };

        public static DeliveryWindowDto DeliveryWindowDto(string name = null, string description = null,
            decimal? price = null, DeliveryWindowType? type = null, DateTime? start = null, DateTime? finish = null,
            bool? available = null) =>
            new DeliveryWindowDto
            {
                Name = name ?? "name",
                Description = description ?? "description",
                Price = price ?? new decimal(49),
                Type = type ?? DeliveryWindowType.Regular,
                Start = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 00),
                Finish = finish ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 00),
                Available = available ?? false,
            };
    }
}
