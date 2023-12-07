using BusinessLogic.Contracts;
using DataSource;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class FiltersService : BaseService, IFiltersService
    {
        public FiltersService(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<object> Find(int id)
        {
            //var item = await this._context.CH_PWP_CATALOGOs.FindAsync(id);

            //return item;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable> FindAll(int id)
        {
            var list = Enumerable.Range(1, id).Select(x => new { id = x, description = x });

            return list;
        }

        public async Task<IEnumerable> FindAll(string key)
        {
            //var items = this._context.CH_PWP_CATALOGOs.Where(x => x.Clave == key).Select(x => new { id = x.Valor, description = x.Descripcion });

            //return items;
            throw new NotImplementedException();
        }

        public Task<IEnumerable> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
