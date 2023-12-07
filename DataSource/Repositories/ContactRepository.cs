using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class ContactRepository : Repository<TCONTACTS>, IContactRepository
    {
        public ContactRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
