using System;

namespace DeliveryManager.Models
{
    public class DeliveryWindow : BaseDeliveryWindow
    {
        public Guid Id { get; set; }

        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableTo { get; set; }

        public TimeSpan ExpectedDeliveryTimeStart { get; set; }

        public TimeSpan ExpectedDeliveryTimeFinish { get; set; }

        public TimeSpan AvailabilityTimeLimit { get; set; }

        public string AvailableDays { get; set; } = @"
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday";
    }
}
