using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class OccupationRepository : Repository<TCOCCUPATIONS>, IOccupationRepository
    {
        public OccupationRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
