using BusinessLogic.Contracts;
using BusinessLogic.Interfaces;
using Contracts.Response;
using DataSource;
using DataSource.Contracts;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class FiltersService : BaseService, IFiltersService
    {
        private ITcSpaceRepository _tcSpaceRepository;
        public FiltersService(AppDbContext appDbContext, ITcSpaceRepository tcSpaceRepository) : base(appDbContext)
        {
            _tcSpaceRepository = tcSpaceRepository;
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

        public async Task<List<TcSpaceResponse>> GetAll()
        {
            var result = await _tcSpaceRepository.Get();

            return result.Select(x => new TcSpaceResponse { description = x.DESCRIPTION, id = x.ID }).ToList();
        }

        Task<IEnumerable> ICatalog.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
