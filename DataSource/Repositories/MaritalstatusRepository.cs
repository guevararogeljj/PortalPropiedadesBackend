using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class MaritalstatusRepository : Repository<TCMARITALSTATUS>, IMaritalstatusRepository
    {
        public MaritalstatusRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
