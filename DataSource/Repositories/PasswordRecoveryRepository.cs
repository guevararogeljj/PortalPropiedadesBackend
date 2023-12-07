using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class PasswordRecoveryRepository : Repository<TPASSWORDRECOVERY>, IPasswordRecoveryRepository
    {
        public PasswordRecoveryRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
