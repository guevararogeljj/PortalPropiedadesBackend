using BusinessLogic.Contracts;
using BusinessLogic.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic.Services
{
    internal class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<Dictionary<string, object>> Authenticate(Dictionary<string, object> user)
        {
            if (!user.Keys.Contains("name") && !user.Keys.Contains("cellphone"))
            {
                user.Add("token", null);
                return user;
            }

            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user["name"].ToString()),
                    //new Claim(ClaimTypes.Email, user["email"].ToString()),
                    new Claim(ClaimTypes.MobilePhone, user["cellphone"].ToString()),
                    new Claim(ClaimTypes.Role, _configuration.GetValue<string>("JWT:Role")),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("JWT:Expiration")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            user.Add("token", tokenhandler.WriteToken(token));

            return user;
        }

        public async Task<Response> ValidateAntiforgery(string data)
        {
            try
            {
                var token = data;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtForm:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "role" && x.Value == _configuration["JwtForm:Role"]).Value;

                return new Response(true);
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
                return new Response(false);
            }
        }

        public async Task<Response> AntiforgeryToken()
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTForm:Key"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Role, _configuration.GetValue<string>("JWTForm:Role")),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("JWTForm:Expiration")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            var response = tokenhandler.WriteToken(token);

            return new Response(true, null, response);
        }

    }
}
