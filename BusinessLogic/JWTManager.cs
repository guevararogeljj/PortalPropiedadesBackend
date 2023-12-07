using DataSource.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BusinessLogic
{
    public static class JWTManager
    {

        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(y =>
            {
                y.RequireHttpsMetadata = false;
                y.SaveToken = true;

                y.Events = new JwtBearerEvents();
                y.Events.OnTokenValidated = (context) =>
                {
                    return LoginValidation(context);
                };


                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("JWT:Issuer"),
                    ValidAudience = configuration.GetValue<string>("JWT:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:Key"))),

                };

            });



            return services;

        }

        private static Task LoginValidation(TokenValidatedContext context)
        {
            try
            {
                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                var instance = userRepository.InstanceObject();
                var token = (context.SecurityToken as JwtSecurityToken);
                instance.CELLPHONE = token.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone").Value;
                var isLogin = userRepository.IsLogin(instance, token.RawData);
                if (isLogin)
                {
                    context.Success();
                    return Task.CompletedTask; ;
                }

                context.Fail("Token expired");
                return Task.CompletedTask; ;
            }
            catch (Exception)
            {

                context.Fail("Token expired - catch");
                return Task.CompletedTask; ;
            }
        }
    }
}
