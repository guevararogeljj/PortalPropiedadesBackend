using BusinessLogic.Contracts;
using BusinessLogic.Models;
using DataSource.Contracts;
using Google.Authenticator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BusinessLogic.Services
{
    internal class TFAService : ITFAService
    {
        private readonly IConfiguration _configuration;
        private readonly IParametersRepository parametersRepository;
        private readonly ILogger<TFAService> _logger;

        public TFAService(IConfiguration configuration, IParametersRepository parametersRepository, ILogger<TFAService> logger)
        {
            this._configuration = configuration;
            this.parametersRepository = parametersRepository;
            this._logger = logger;
        }
        public async Task<Response> GenerateRegisterCode(string email, string code, string apptitle)
        {
            try
            {
                var key = this.GenerateKey(email);
                var bytes = Encoding.UTF8.GetBytes(key);
                TwoFactorAuthenticator auth = new TwoFactorAuthenticator();
                var setupcode = auth.GenerateSetupCode(apptitle, email, bytes);


                var result = new { url = setupcode.QrCodeSetupImageUrl, manualkey = setupcode.ManualEntryKey, };

                return new Response(true, "", result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error en el registro");
            }
        }



        public async Task<Response> ValidateCode(string email, string secretcode, string appcode)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool result = tfa.ValidateTwoFactorPIN(this.GenerateKey(email), appcode);

            if (result)
            {
                return new Response(true, "Codigo valido");
            }

            this._logger.LogInformation($"Codigo de app fallido en la cuenta: {email}");
            return new Response(false, "Codigo no valido");

        }

        private string GenerateKey(string email)
        {
            var secretcode = this.parametersRepository.GetParameter<string>("TWPFACTORAUTHENTICATION", "TWOFACTORSECRETCODE");
            return $"{secretcode}-{email}";
        }
    }
}
