using System.Collections;

namespace BusinessLogic.Interfaces
{
    public interface ICatalog
    {
        Task<object> Find(int id);
        Task<IEnumerable> FindAll(int id);

        Task<IEnumerable> FindAll(string key);

        Task<IEnumerable> GetAll();

    }
}
