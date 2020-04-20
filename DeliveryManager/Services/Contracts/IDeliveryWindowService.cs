using DeliveryManager.Models;
using DeliveryManager.Models.Requests;
using System;
using System.Collections.Generic;

namespace DeliveryManager.Services.Contracts
{
    public interface IDeliveryWindowService
    {
        IEnumerable<DeliveryWindowDto> Get(DateTime currentDate, int horizon);
        Guid Add(CreateWindowRequest request);
        DeliveryWindowDto Update(Guid id, UpdateWindowRequest request);
    }
}
