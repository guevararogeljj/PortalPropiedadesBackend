using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class UserPropertiesRepository : Repository<TRUSERPROPERTIES>, IUserPropertiesRepository
    {
        public UserPropertiesRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public void DeleteRange(IEnumerable<TRUSERPROPERTIES> list)
        {
            this._dbset.RemoveRange(list);
        }

        public TRUSERPROPERTIES FindByUserAndProperty(int iduser, int idproperty)
        {
            return this._dbset.Where(x => x.IDUSER == iduser && x.IDPROPERTY == idproperty).FirstOrDefault();
        }

        public List<TRUSERPROPERTIES> GetAllByUser(int iduser)
        {
            return this._dbset.Where(x => x.IDUSER == iduser).ToList();
        }
    }
}
