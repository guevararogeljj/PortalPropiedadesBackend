//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

//namespace DataSource.Config
//{
//    public partial class AppDbContext : DbContext
//    {
//        protected readonly IConfiguration _configuration;

//        public AppDbContext(IConfiguration configuration)
//        {
//            this._configuration = configuration;
//        }
//        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
//        {
//            this._configuration = configuration;
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            string connectionString = _configuration.GetConnectionString("Default");

//            optionsBuilder
//                .UseLazyLoadingProxies()
//                .UseSqlServer(connectionString, x => x.UseNetTopologySuite());
//        }
//    }
//}
