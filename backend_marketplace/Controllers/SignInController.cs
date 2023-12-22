using backend_marketplace.Models;
using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_marketplace.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ILogger<SignInController> _logger;
        private readonly ISignInUserService _signInUserService;
        private readonly IPropertiesService _propertiesService;


        public SignInController(ISignInUserService signInService, IPropertiesService propertiesService, ILogger<SignInController> logger)
        {
            this._signInUserService = signInService;
            this._logger = logger;
            this._propertiesService = propertiesService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Login login)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");
                return new JsonResult(new { success = false, message = "Datos invalidos", result = "" });
            }

            var result = await this._signInUserService.SignIn(login.ToDictionary());
            this._logger.LogInformation($"Acceso exitoso de usuario {login.Cellphone}");

            return new JsonResult(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(UserProfile login)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "Usuario no valido" });
            }

            this._logger.LogInformation($"Logout del usuario: {login.Cellphone}");
            var result = this._signInUserService.SignOut(login.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("profile")]
        [Authorize]
        public async Task<IActionResult> Profile(UserProfile user)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");
                return new JsonResult(new { result = "", success = false, message = "usuario no valido" });
            }

            var result = (await this._signInUserService.Profile(user.ToDictionary()));

            return new JsonResult(result);
        }

        [HttpPost("favorites")]
        [Authorize]
        public async Task<IActionResult> Favorites(UserProfile user)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._propertiesService.Favorites(user.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("addfavorite")]
        [Authorize]
        public async Task<IActionResult> AddFavorite(Favorite user)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._propertiesService.AddFavorite(user.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("removefavorite")]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(Favorite user)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._propertiesService.RemoveFavorite(user.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword credencial)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.ChangePassword(credencial.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("removeallfavorites")]
        [Authorize]
        public async Task<IActionResult> RemoveAllFavorites(Logout user)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._propertiesService.RemoveAllFavorite(user.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("changeemail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmail credencial)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.ChangeEmail(credencial.ToDictionary());

            return new JsonResult(result.Result);
        }

        [HttpPost("updoubleauth")]
        [Authorize]
        public async Task<IActionResult> UpDoubleAuth(Logout credencial)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.UpDoubleAuth(credencial.ToDictionary());

            return new JsonResult(result.Result);
        }

        //[Authorize]
        [HttpPost("validatedoubleauth")]
        public async Task<IActionResult> ValidateDoubleAuth(TwoFactorAuthentication data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.ValidateDoubleAuth(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("updoubleauthstatus")]
        public async Task<IActionResult> UpDoubleAuthStatus(TwoFactorAuthentication data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.UpDoubleAuthStatus(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("downdoubleauthstatus")]
        public async Task<IActionResult> DownDoubleAuthStatus(TwoFactorAuthentication data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.DownDoubleAuthStatus(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("changecellphone")]
        public async Task<IActionResult> ChangeCellphone(ChangeCellphone data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.ChangeCellphone(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("changecellphonesmscode")]
        public async Task<IActionResult> ChangeCellphoneSmsCode(ChangeCellphone data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.SendChangeCellphoneCodeSms(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("validatechangecellphonesmscode")]
        public async Task<IActionResult> ValidateChangeCellphoneSmsCode(ChangeCellphoneCode data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = this._signInUserService.ValidateChangeCellphoneSmsCode(data.ToDictionary());

            return new JsonResult(result.Result);
        }

        [Authorize]
        [HttpPost("addcontractdata")]
        public async Task<IActionResult> AddContractData(ContractInfo data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = await this._signInUserService.ContractInfo(data.ToDictionary());

            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost("ndastatus")]
        public async Task<IActionResult> NDASignedStatus(Logout data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
            }

            var result = await this._signInUserService.NDASignedStatus(data.ToDictionary());

            return new JsonResult(result);
        }

        [Authorize]
        [HttpPut("UpdateDataUser")]
        public async Task<IActionResult> UpdateDataUser(UserData data)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogError($"Intento de actualizar datos fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");
                return StatusCode(413, new JsonResult(new { result = "", success = false, message = "informacion no valida" })); 
            }

            var result = await this._signInUserService.UpdateDataUser(data.ToDictionary());

            return new JsonResult(result);
        }

        //[HttpPost("generatecontract")]
        //public async Task<IActionResult> GenerateContract(Logout data)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

        //        return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
        //    }

        //    var result = await this._signInUserService.GenerateContract(data.ToDictionary());

        //    return new JsonResult(result);
        //}


    }
}
