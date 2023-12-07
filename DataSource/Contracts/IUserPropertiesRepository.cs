using DataSource.Entities;
using DataSource.Interfaces;

namespace DataSource.Contracts
{
    public interface IUserPropertiesRepository : IRepository<TRUSERPROPERTIES>
    {
        public TRUSERPROPERTIES FindByUserAndProperty(int iduser, int idproperty);

        public List<TRUSERPROPERTIES> GetAllByUser(int iduser);

        public void DeleteRange(IEnumerable<TRUSERPROPERTIES> list);
    }


}
