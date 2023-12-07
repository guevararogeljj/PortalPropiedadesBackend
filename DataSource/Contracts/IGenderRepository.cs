using DataSource.Entities;
using DataSource.Interfaces;

namespace DataSource.Contracts
{
    public interface IGenderRepository : IRepository<TCGENDER>
    {
        public TCGENDER FindByDescription(string description);
    }
}
