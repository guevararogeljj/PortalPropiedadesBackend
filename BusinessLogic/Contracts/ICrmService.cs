using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface ICrmService
    {
        public Task<Response> ClientToCrm(Dictionary<string, object> data);
    }
}
