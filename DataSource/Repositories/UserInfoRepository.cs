using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataSource.Repositories
{
    internal class UserInfoRepository : Repository<TUSERSINFO>, IUserInfoRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserInfoRepository(DbContextOptions<AppDbContext> options, ILogger<UserRepository> logger) : base(options)
        {
            this._logger = logger;
        }
    }
}
