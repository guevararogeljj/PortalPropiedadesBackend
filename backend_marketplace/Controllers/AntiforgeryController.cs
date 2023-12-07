using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace backend_marketplace.Controllers
{
    [ApiController]
    [Route("api/antiforgery")]
    public class AntiforgeryController : Controller
    {
        private readonly IAntiforgery _antiforegery;
        private readonly IJWTService _jwtService;

        public AntiforgeryController(IAntiforgery antiforgety, IJWTService jwtService)
        {
            this._antiforegery = antiforgety;
            this._jwtService = jwtService;
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> get()
        {
            var token = _antiforegery.GetAndStoreTokens(HttpContext);
            var resquestToken = token.RequestToken;
            Response.Cookies.Append("XSRF-REQUEST-TOKEN", resquestToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                MaxAge = TimeSpan.FromMinutes(60),
                SameSite = SameSiteMode.None,
                IsEssential = true,

            });

            var jwtpken = await _jwtService.AntiforgeryToken();

            return new JsonResult(jwtpken);
        }
    }
}
