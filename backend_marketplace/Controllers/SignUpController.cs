using backend_marketplace.Models;
using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using File = backend_marketplace.Models.File;

namespace backend_marketplace.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : BaseController
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly ISignUpUserService _signUpUserService;
        private readonly IConfiguration _configuration;
        private readonly IJWTService _jwtservice;

        public SignUpController(ISignUpUserService signUpService, ILogger<SignUpController> logger, IConfiguration configuration, IJWTService jWTService) : base(logger)
        {
            this._logger = logger;
            this._signUpUserService = signUpService;
            this._configuration = configuration;
            this._jwtservice = jWTService;
        }

        [HttpPost]
        public async Task<IActionResult> post(User user)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                this._logger.LogError($"Intento de dar de alta un nuevo usuario ha sido fallido, ip: {Request.HttpContext.Connection.LocalIpAddress}");
                return StatusCode(413);
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            var result = this._signUpUserService.AddNewUser(user.ToDictionary());
            this._logger.LogInformation($"{result.ToString()}");

            if (result.Message == null || result.Message == "El password no valido.")
                return StatusCode(413);

            return new JsonResult(result);
        }

        [HttpPost("sendcodesms")]
        public async Task<IActionResult> SendCodeSms(Logout user)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch 
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }           

            var result = _signUpUserService.SendCodeSms(user.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("validatecodesms")]
        public async Task<IActionResult> ValidateCodeSms(SmsCode sms)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            var result = _signUpUserService.ValidateSmsCode(sms.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("validateidentification")]
        public async Task<IActionResult> ValidateIdentification(IEnumerable<File> files)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            List<Dictionary<string, object>> filesDic = new List<Dictionary<string, object>>();

            foreach (var item in files)
            {
                filesDic.Add(item.ToDictionary());
            }

            var result = _signUpUserService.ValidateIdentification(filesDic);

            return new JsonResult(result.Result);
        }


        [HttpPost("userinformation")]
        public async Task<IActionResult> UserInformation(Logout data)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            //var result = _signUpUserService.UserInformation(data.ToDictionary());
            var result = await _signUpUserService.UserInformation(data.ToDictionary());

            //return new JsonResult(result.Result);
            return new JsonResult(result);
        }

        [HttpPost("confirmuserinformation")]
        public async Task<IActionResult> UpdateUserInformation(UpdateTerms data)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            var result = _signUpUserService.UpdateUserInformation(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        private IEnumerable<string> Errors()
        {
            var res = ModelState.Select(x => new { id = x.Key, message = x.Value.Errors.Select(y => y.ErrorMessage).FirstOrDefault() });
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(o => o.ErrorMessage);
            return errors;
        }

        [HttpGet("emailvalidation")]
        public async Task<IActionResult> EmailValidation(string email, string code)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            //await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Email", email);
            data.Add("Code", code);

            var result = _signUpUserService.ValidateCodeEmail(data);

            return new JsonResult(result.Result);
        }


        [Authorize]
        [HttpPost("sendcodeemail")]
        public async Task<IActionResult> SendCodeEmail(Logout data)
        {
            try {

                if (!ModelState.IsValid)
                {
                    IEnumerable<string> errors = Errors();
                    return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
                }

                //await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = _signUpUserService.SendCodeEmail(data.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex)
            {
                Unauthorized();
            }
            
        }

        [HttpPost("sendpasswordrecovery")]
        public async Task<IActionResult> SendPasswordRecovery(Logout data)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            var result = _signUpUserService.PasswordRecovery(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("passwordrecovery")]
        public async Task<IActionResult> PasswordRecovery(PasswordRecovery data)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            try
            {
                await ValidateAntiforgeryToken(HttpContext, _configuration["JwtForm:HeaderName"], _jwtservice);
            }
            catch
            {
                IEnumerable<string> errors = Errors();
                return new JsonResult(errors) { StatusCode = StatusCodes.Status406NotAcceptable };
            }

            var result = _signUpUserService.PasswordRecoveryValidate(data.ToDictionary());

            return new JsonResult(result.Result);
        }


        [HttpPost("acceptterms")]
        public async Task<IActionResult> AcceptTemsContract([FromBody] JsonElement data)
        {

            var apikeyname = _configuration.GetValue<string>("WebdoxServices:APIKEYNAME");
            var apikeyheaderservice = Request.Headers.Where(x => x.Key.Equals(apikeyname)).FirstOrDefault();
            var validapikey = _configuration.GetValue<string>("WebdoxServices:WebdoxApikey");

            if (validapikey.Equals(apikeyheaderservice))
            {
                _logger.LogError(string.Format("Intento que ejecucion de servicio webdox. cliente : {0}", Request.HttpContext.Connection.RemoteIpAddress));
                return new JsonResult(new { success = false, message = "El Api Key no es valida" });
            }

            _logger.LogInformation("Servicio de webodx webhook ha sido invocado.");
            _logger.LogInformation(data.ToString());

            var result = this._signUpUserService.UpdateWebdoxRestration(data.ToString());



            return new JsonResult(data);
        }
    }
}
