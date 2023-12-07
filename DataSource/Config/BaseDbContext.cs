
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataSource.Config
{
    public class BaseDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public BaseDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public BaseDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("Default");

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString, x => x.UseNetTopologySuite());
        }

    }
}
