using DataSource.Entities;
using DataSource.Interfaces;

namespace DataSource.Contracts
{
    public interface IWebdoxRequestRepository : IRepository<WEBDOXREQUEST>
    {
        WEBDOXREQUEST FindRequestByIdWebdox(WEBDOXREQUEST entity);

        List<WEBDOXREQUEST> FindAllRequestByIdWebdox(string entity);
    }
}
