using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using DeliveryManager.Infrastructure.Attributes;
using DeliveryManager.Models;
using DeliveryManager.Models.Requests;
using DeliveryManager.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeliveryManager.Controllers
{
    [ApiVersion(Constants.ApiVersions.V1)]
    [Route(Constants.ApiSegment.Api + "/window")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DeliveryWindowController : Controller
    {
        private readonly IDeliveryWindowService _windowService;

        public DeliveryWindowController(IDeliveryWindowService windowService)
        {
            _windowService = windowService;
        }

        /// <remarks>
        /// Get delivery window
        /// 
        /// Used to retrieve a existing delivery window.
        /// <br/>The response will contain the delivery window.
        /// 
        ///     GET /delivery-manager/window?currentDate=2020-01-01&horizon=7
        /// 
        /// </remarks>
        /// <param name="id">Delivery window Id to get</param>
        /// <returns>Delivery window</returns>
        [HttpGet]
        [Route("", Name = DeliveryManagerRoutes.Get)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(IEnumerable<DeliveryWindowDto>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        [ValidateActionParameters]
        public IActionResult Get([Required]DateTime currentDate, int horizon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (horizon < 0)
                return BadRequest("The horizon parameter cannot be less than 0");

            if(horizon > 50)
                return BadRequest("The horizon parameter cannot be more than 50");

            var response = _windowService.Get(currentDate, horizon);
            return response != null ? (IActionResult)Ok(response) : NotFound();
        }

        /// <remarks>
        /// Add delivery window
        /// 
        /// Used to add a delivery window.
        /// <br/>The response will contain Id of the added delivery window.
        /// 
        ///     POST /delivery-manager/window
        /// 
        /// </remarks>
        /// <param name="id">Delivery window model</param>
        /// <returns>Id of the delivery window</returns>
        [HttpPost]
        [Route("", Name = DeliveryManagerRoutes.Add)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        public IActionResult Add(CreateWindowRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _windowService.Add(request);
            return Ok(response);
        }

        /// <remarks>
        /// Update delivery window
        /// 
        /// Used to update a delivery window by Id.
        /// <br/>The response will contain the updated delivery window.
        /// 
        ///     PATCH /delivery-manager/window/0d2b3411-6124-453c-a5c0-7316eb3f355c
        /// 
        /// </remarks>
        /// <param name="id">The delivery window model</param>
        /// <returns>The delivery window</returns>
        [HttpPatch]
        [Route("{id}", Name = DeliveryManagerRoutes.Update)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(DeliveryWindowDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        public IActionResult Update(Guid id, [FromBody] UpdateWindowRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id == Guid.Empty)
                return BadRequest("The id parameter should be defined");

            var response = _windowService.Update(id, request);
            return response != null ? (IActionResult)Ok(response) : NotFound();
        }
    }
}