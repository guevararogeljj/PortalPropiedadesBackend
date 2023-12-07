using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface IWebdoxRequestService
    {
        Task<Response> AddNewRequestNDA(int order, int user, string formwebdoxid, string jsonresponse, string description, bool status = false);

        Task<Response> WebdoxReponse(string idwebodx, string jsonresponse);
    }
}
