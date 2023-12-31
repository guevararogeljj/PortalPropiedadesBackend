﻿using backend_marketplace.Models;
using BusinessLogic.Contracts;
using Contracts.Request;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend_marketplace.Controllers
{
    [Route("api/properties")]
    [Produces("application/json")]
    [ApiController]
    public class PropertiesController : BaseController
    {
        private readonly IPropertiesService _propertiesService;

        public PropertiesController(IPropertiesService _propertiesService, ILogger<PropertiesController> logger) : base(logger)
        {
            this._propertiesService = _propertiesService;
        }

        [HttpPost("range")]
        public async Task<IActionResult> RangeProperties([FromBody]PropertiesFilterDto request)
        {

            _logger.LogInformation($"Ingreso al api range");
            var result = await _propertiesService.PropertiesRange(request);
            if(result.Succes)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> Details(string id)
        {
            var result = _propertiesService.PropertyDetails(id);

            if (result.IsFaulted)
            {
                var message = (result.Exception != null ? result.Exception.Message.ToString() : result.Exception.ToString());
                this._logger.LogWarning($"result: {result.Result} - Exception: {message}");
                return new JsonResult(result.Exception != null ? result.Exception.Message : "No se encontraron resultados") { StatusCode = StatusCodes.Status400BadRequest };
            }
            if (result.Result == null)
            {
                string message = "No se encontraron resultados.";
                this._logger.LogInformation($"result: {result.Result} - Error: {message}");
                return new JsonResult(result.Exception != null ? result.Exception.Message : message) { StatusCode = StatusCodes.Status400BadRequest };
            }

            this._logger.LogInformation($"result: {result.Result}");
            return new JsonResult(result.Result) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPost("legaldetails")]
        [Authorize]
        public async Task<IActionResult> LegalDetails(Property prop)
        {
            var result = this._propertiesService.LegalDetails(prop.ToDictionary());

            if (result.Result == null || result.IsFaulted)
            {
                string message = "No se encontraron resultados.";
                this._logger.LogInformation($"result: {result.Result} - Error: {message}");
                return new JsonResult(result.Exception != null ? result.Exception.Message : message) { StatusCode = StatusCodes.Status400BadRequest };
            }

            return new JsonResult(result.Result) { StatusCode = StatusCodes.Status200OK };
        }

    }
}
