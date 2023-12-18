using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace backend_marketplace.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            this._logger = logger;
        }
        protected IActionResult JsonResponse<T>(Task<T> result)
        {
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


        protected async Task ValidateAntiforgeryToken(HttpContext context, string headername, IJWTService jwtService)
        {
            var header = context.Request.Headers.Where(x => x.Key == headername).Select(y => y.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                throw new Exception("bad request");
            }

            var result = await jwtService.ValidateAntiforgery(header);

            if (result.Success == false)
            {
                throw new Exception("Token resquest not valid!");
            }
        }
    }
}
