using backend_marketplace.Models;
using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_marketplace.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        private readonly IConfiguration _configuration;
        private readonly IJWTService _jwtservice;

        public ContactController(IContactService contactService, IConfiguration configuration, IJWTService jWTService, ILogger<ContactController> logger) : base(logger)
        {
            this._contactService = contactService;
            this._configuration = configuration;
            this._jwtservice = jWTService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Datos no validos.", result = "" });
            }

            await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);

            var result = await this._contactService.AddNewContact(contact.ToDictionary());

            return new JsonResult(result);
        }


        [HttpGet("shareproperty")]
        public async Task<IActionResult> ShareProperty(string email, int creditnumber)
        {

            if (!ModelState.IsValid)
            {
                return new JsonResult(false);
            }

            await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Email", email);
            properties.Add("CreditNumber", creditnumber);

            this._contactService.ShareProperty(properties);


            return new JsonResult(true);
        }

        [Authorize]
        [HttpGet("requestinformation")]
        public async Task<IActionResult> RequestInfoProperty(string email, int reference)
        {

            if (!ModelState.IsValid)
            {
                return new JsonResult(false);
            }

            await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Email", email);
            properties.Add("CreditNumber", reference);

            this._contactService.RequestInfoProperty(properties);


            return new JsonResult(true);
        }
    }
}
