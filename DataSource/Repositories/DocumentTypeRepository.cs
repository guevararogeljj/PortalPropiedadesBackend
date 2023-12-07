using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class DocumentTypeRepository : Repository<TCDOCUMENTTYPE>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public TCDOCUMENTTYPE FindByDescription(string description)
        {
            return this._dbset.Where(x => x.DESCRIPTION == description).First();
        }
    }
}
