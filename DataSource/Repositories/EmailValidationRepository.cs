using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class EmailValidationRepository : Repository<TEMAILVALIDATION>, IEmailValidationRepository
    {
        public EmailValidationRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


    }
}
