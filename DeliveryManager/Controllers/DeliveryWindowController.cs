using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DeliveryManager.Infrastructure.Attributes;
using DeliveryManager.Models;
using DeliveryManager.Models.Requests;
using DeliveryManager.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

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
        ///     GET /delivery-manager/window?currentDate=2020-01-01&horizon=7&api-version=1.0
        /// 
        /// </remarks>
        /// <param name="id">Delivery window Id to get</param>
        /// <returns>Delivery window</returns>
        [HttpGet]
        [Route("", Name = DeliveryManagerRoutes.Get)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(IEnumerable<DeliveryWindowDto>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        [ValidateActionParameters]
        public async Task<IActionResult> Get([Required]DateTime currentDate, int horizon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _windowService.Get(currentDate, horizon);
            return response != null ? (IActionResult)Ok(response) : NotFound();
        }

        /// <remarks>
        /// Add delivery window
        /// 
        /// Used to add a delivery window by Id.
        /// <br/>The response will contain Id of the added delivery window.
        /// 
        ///     POST /delivery-manager/window?api-version=1.0
        /// 
        /// </remarks>
        /// <param name="id">Delivery window model</param>
        /// <returns>Id of the delivery window</returns>
        [HttpPost]
        [Route("", Name = DeliveryManagerRoutes.Add)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        public async Task<IActionResult> Add(CreateWindowRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _windowService.Add(request);
            return Ok(response);
        }

        /// <remarks>
        /// Update delivery window
        /// 
        /// Used to update a delivery window by Id.
        /// <br/>The response will contain Id of the added delivery window.
        /// 
        ///     PATCH /delivery-manager/window/0D2B3411-6124-453C-A5C0-7316EB3F355C?api-version=1.0
        /// 
        /// </remarks>
        /// <param name="id">Delivery window model</param>
        /// <returns>Delivery window</returns>
        [HttpPatch]
        [Route("{id}", Name = DeliveryManagerRoutes.Update)]
        [SwaggerResponse((int)HttpStatusCode.OK, "Delivery window found", typeof(DeliveryWindowDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Delivery window not found")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWindowRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _windowService.Update(id, request);
            return response != null ? (IActionResult)Ok(response) : NotFound();
        }
    }
}