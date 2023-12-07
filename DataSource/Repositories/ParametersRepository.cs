using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class ParametersRepository : IParametersRepository
    {

        protected readonly AppDbContext _context;
        protected readonly DbSet<TPARAMETERS> _dbset;

        public ParametersRepository(DbContextOptions<AppDbContext> options)
        {
            this._context = new AppDbContext(options);
            this._dbset = _context.Set<TPARAMETERS>();
        }

        public T GetParameter<T>(string groupname, string name)
        {
            var result = this._dbset.Where(x => x.GROUPNAME.Equals(groupname) && x.NAME.Equals(name)).FirstOrDefault();

            return (T)Convert.ChangeType(result.VALUE, typeof(T));
        }

        public Dictionary<string, object> GetParameters(string groupname)
        {
            var result = this._dbset.Where(x => x.GROUPNAME.Equals(groupname)).ToDictionary(x => Convert.ToString(x.NAME), x => (object)x.VALUE);

            return result;
        }

        public List<T> GetParameters<T>(string groupname, string name)
        {
            var result = this._dbset.Where(x => x.GROUPNAME.Equals(groupname) && x.NAME.Equals(name)).Select(x => x.VALUE).ToList();

            var convert = result.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();

            return convert;
        }
    }
}
