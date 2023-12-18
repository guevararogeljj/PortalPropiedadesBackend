using BusinessLogic.Interfaces;
using Contracts.Response;

namespace BusinessLogic.Contracts
{
    public interface IFiltersService : ICatalog
    {
        Task<List<TcSpaceResponse>> GetAll();
    }
}
