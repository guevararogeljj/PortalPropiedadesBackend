using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class DocumentsRepository : Repository<TDOCUMENTS>, IDocumentsRepository
    {
        public DocumentsRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
