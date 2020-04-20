using AutoMapper;
using DeliveryManager.Models;
using DeliveryManager.Models.Requests;
using DeliveryManager.Repositories.Contracts;
using DeliveryManager.Services.Contracts;
using System;
using System.Collections.Generic;

namespace DeliveryManager.Services
{
    public class DeliveryWindowService : IDeliveryWindowService
    {
        private readonly IDeliveryWindowRepo _repo;
        private readonly IMapper _mapper;

        public DeliveryWindowService(IDeliveryWindowRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<DeliveryWindowDto> Get(DateTime currentDate, int horizon)
        {
            List<DeliveryWindowDto> deliveryWindows = new List<DeliveryWindowDto>();
            for (int i = 0; i <= horizon; i++)
            {
                var date = currentDate.AddDays(i);
                var models = _repo.Get(currentDate);

                foreach (var model in models)
                {
                    var available = model.Type switch
                    {
                        DeliveryWindowType.Urgent =>
                            date.Date != currentDate.Date ||
                            date.TimeOfDay >= Constants.UrgentDeliverySlot.From &&
                            date.TimeOfDay <= Constants.UrgentDeliverySlot.To,
                        DeliveryWindowType.Regular =>
                            date.Date != currentDate.Date ||
                            (model.ExpectedDeliveryTimeStart.TotalMilliseconds - date.TimeOfDay.TotalMilliseconds) >=
                                model.AvailabilityTimeLimit.TotalMilliseconds,
                        _ => false,
                    };

                    deliveryWindows.Add(new DeliveryWindowDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Type = model.Type,
                        Start = date.Date.Add(model.ExpectedDeliveryTimeStart),
                        Finish = date.Date.Add(model.ExpectedDeliveryTimeFinish),
                        Available = available
                    });
                }
            }

            return deliveryWindows;
        }

        public Guid Add(CreateWindowRequest request)
        {
            return _repo.Add(_mapper.Map<DeliveryWindow>(request));
        }

        public DeliveryWindowDto Update(Guid id, UpdateWindowRequest request)
        {
            var model = _repo.Update(id, _mapper.Map<DeliveryWindow>(request));
            return _mapper.Map<DeliveryWindowDto>(model);
        }
    }
}
