using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface IJWTService
    {
        Task<Dictionary<string, object>> Authenticate(Dictionary<string, object> user);
        Task<Response> AntiforgeryToken();
        Task<Response> ValidateAntiforgery(string value);

    }
}
