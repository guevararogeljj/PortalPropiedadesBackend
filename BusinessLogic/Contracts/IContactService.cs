using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface IContactService
    {
        Task<Response> AddNewContact(Dictionary<string, object> data);

        void ShareProperty(Dictionary<string, object> data);

        void RequestInfoProperty(Dictionary<string, object> data);

    }
}
