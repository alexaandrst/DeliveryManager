using DeliveryManager.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManager.Models
{
    public class BaseDeliveryWindow : IDeliveryWindow
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DeliveryWindowType Type { get; set; }

        [NotMapped]
        public bool Available { get; set; }
    }
}
