using DataSource.Entities;
using DataSource.Interfaces;
using LinqKit;

namespace DataSource.Contracts
{
    public interface IPropertyRepository : IRepository<TPROPERTIES>
    {
        TPROPERTIES FindPropertyByCreditNumber(string creditnumber);

        Dictionary<string, object> SharePropertyInfo(string creditnumber);

        IEnumerable<TPROPERTIES> AvailableProperties();

        IEnumerable<TPROPERTIES> PageSizeAvailableProperties(int? index, int? pagesize, ExpressionStarter<TPROPERTIES> where);

        int? CountAvailableProperties(int? index, int? pagesize, ExpressionStarter<TPROPERTIES> where = null);
    }
}
