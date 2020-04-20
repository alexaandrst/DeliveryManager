using DeliveryManager.Models;
using System;
using System.Collections.Generic;

namespace DeliveryManager.Repositories.Contracts
{
    public interface IDeliveryWindowRepo
    {
        IEnumerable<DeliveryWindow> Get(DateTime currentDate);
        Guid Add(DeliveryWindow window);
        DeliveryWindow Update(Guid id, DeliveryWindow window);
    }
}
