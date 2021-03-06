﻿namespace DeliveryManager.Models.Contracts
{
    public interface IDeliveryWindow
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DeliveryWindowType Type { get; set; }
    }
}
