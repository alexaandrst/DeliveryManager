using DeliveryManager.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryManager.Models.Requests
{
    public class BaseWindowRequest : IDeliveryWindow
    {
        [Required]
        [StringLength(5)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Range(0, 999.99)]
        public decimal Price { get; set; }

        [Required]
        public DeliveryWindowType Type { get; set; }

        [Required]
        public TimeSpan ExpectedDeliveryTimeStart { get; set; }

        [Required]
        public TimeSpan ExpectedDeliveryTimeFinish { get; set; }

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        public TimeSpan AvailabilityTimeLimit { get; set; }

        public string AvailableDays { get; set; } = @$"
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday";
    }
}
