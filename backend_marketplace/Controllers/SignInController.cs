using backend_marketplace.Models;
using BusinessLogic.Contracts;
using DataSource.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

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

            if (result.Success!.Value)
            {
                this._logger.LogInformation($"Acceso exitoso de usuario {login.Cellphone}");
                return new JsonResult(result);
            }
            else
            {
                if (result.Message!.Contains("Usuario/contraseña invalida"))
                {
                    return StatusCode(StatusCodes.Status404NotFound, result);
                }
                else if (result.Message!.Contains("Ha exedido el numero de intentos intente mas tarde"))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
                else if(result.Message!.Contains("Este usuario no fue validado, por favor intenta recuperar tu contraseña"))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }
            }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");
                    return new JsonResult(new { result = "", success = false, message = "usuario no valido" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (user.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = (await this._signInUserService.Profile(user.ToDictionary()));

                return new JsonResult(result);


            }
            catch (Exception ex) 
            {
                return Unauthorized();            
            }            
        }

        [HttpPost("favorites")]
        [Authorize]
        public async Task<IActionResult> Favorites(UserProfile user)
        {
            try {

                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }
                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (user.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._propertiesService.Favorites(user.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);

            }            
        }

        [HttpPost("addfavorite")]
        [Authorize]
        public async Task<IActionResult> AddFavorite(Favorite user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (user.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");


                var result = this._propertiesService.AddFavorite(user.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch (Exception ex) 
            { 
                return Unauthorized();            
            }            
        }

        [HttpPost("removefavorite")]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(Favorite user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (user.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._propertiesService.RemoveFavorite(user.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex) {
            
                return Unauthorized();
            }
            
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword credencial)
        {
            try {

                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (credencial.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.ChangePassword(credencial.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }            
        }

        [HttpPost("removeallfavorites")]
        [Authorize]
        public async Task<IActionResult> RemoveAllFavorites(Logout user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }
                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (user.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._propertiesService.RemoveAllFavorite(user.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch (Exception ex) 
            { 
                return Unauthorized();
            }
            
        }

        [HttpPost("changeemail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmail credencial)
        {

            try {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var emailClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (emailClaimValue != null && credencial.Email != emailClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.ChangeEmail(credencial.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch {
            
                return Unauthorized();            
            }
            
        }

        [HttpPost("updoubleauth")]
        [Authorize]
        public async Task<IActionResult> UpDoubleAuth(Logout credencial)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (credencial.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.UpDoubleAuth(credencial.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex) {
            
               return Unauthorized();
            }
            
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
            try
            {

                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");


                var result = this._signInUserService.UpDoubleAuthStatus(data.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch (Exception ex) 
            {
                return Unauthorized();
            
            }            
        }

        [Authorize]
        [HttpPost("downdoubleauthstatus")]
        public async Task<IActionResult> DownDoubleAuthStatus(TwoFactorAuthentication data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogInformation($"Intento de acceso al portal ha fallado ip de acceso: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.DownDoubleAuthStatus(data.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch (Exception ex) 
            { 
            
                return Unauthorized();            
            }
            
        }

        [Authorize]
        [HttpPost("changecellphone")]
        public async Task<IActionResult> ChangeCellphone(ChangeCellphone data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.ChangeCellphone(data.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch (Exception ex) 
            {
                return Unauthorized();           
            
            }           
        }

        [Authorize]
        [HttpPost("changecellphonesmscode")]
        public async Task<IActionResult> ChangeCellphoneSmsCode(ChangeCellphone data)
        {
            try {

                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.SendChangeCellphoneCodeSms(data.ToDictionary());

                return new JsonResult(result.Result);

            }
            catch(Exception ex)
            {
                return Unauthorized();
            }            
        }

        [Authorize]
        [HttpPost("validatechangecellphonesmscode")]
        public async Task<IActionResult> ValidateChangeCellphoneSmsCode(ChangeCellphoneCode data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = this._signInUserService.ValidateChangeCellphoneSmsCode(data.ToDictionary());

                return new JsonResult(result.Result);
            }
            catch (Exception ex) 
            { 
            
                return Unauthorized() ;
            }
            
        }

        [Authorize]
        [HttpPost("addcontractdata")]
        public async Task<IActionResult> AddContractData(ContractInfo data)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }

                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = await this._signInUserService.ContractInfo(data.ToDictionary());

                return new JsonResult(result);
            }
            catch (Exception ex) 
            { 
                return Unauthorized();
            }
            
        }

        [Authorize]
        [HttpPost("ndastatus")]
        public async Task<IActionResult> NDASignedStatus(Logout data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de cambio de celular fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    return new JsonResult(new { result = "", success = false, message = "informacion no valida" });
                }
                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = await this._signInUserService.NDASignedStatus(data.ToDictionary());

                return new JsonResult(result);


            }
            catch (Exception ex) 
            { 
                return Unauthorized() ;
            
            }
            
        }

        [Authorize]
        [HttpPut("UpdateDataUser")]
        public async Task<IActionResult> UpdateDataUser(UserData data)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Intento de actualizar datos fallido: {data.ToString()},  IP: {Request.HttpContext.Connection.RemoteIpAddress}");
                    return StatusCode(413, new JsonResult(new { result = "", success = false, message = "informacion no valida" }));
                }
                var cellClaimValue = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                if (data.Cellphone != cellClaimValue)
                    throw new Exception("No autorizado");

                var result = await this._signInUserService.UpdateDataUser(data.ToDictionary());
                if (result.Success!.Value)
                    this._logger.LogInformation($"Se actualizaron datos de {data.Cellphone}");
                else
                {
                    if (result.Message!.Contains("Informacion no valida"))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, result);
                    }
                    else if (result.Message!.Contains("Ha ocurrido un error al actualizar datos al guardar."))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, result);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
                return new JsonResult(result);
            }
            catch (Exception ex) 
            { 
            
                return Unauthorized();
            }
            
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
