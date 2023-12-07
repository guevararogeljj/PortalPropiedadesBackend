
using DataSource.Entities;
using DataSource.Interfaces;

namespace DataSource.Contracts
{
    public interface IDocumentTypeRepository : IRepository<TCDOCUMENTTYPE>
    {
        TCDOCUMENTTYPE FindByDescription(string description);
    }
}
