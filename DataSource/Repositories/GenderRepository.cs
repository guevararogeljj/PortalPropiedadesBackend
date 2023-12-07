using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class GenderRepository : Repository<TCGENDER>, IGenderRepository
    {
        public GenderRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public TCGENDER FindByDescription(string description)
        {
            return this._dbset.Where(x => x.DESCRIPTION == description).First();
        }
    }
}
