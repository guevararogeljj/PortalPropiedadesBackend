using BusinessLogic.Contracts;
using DataSource;
using DataSource.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections;


namespace BusinessLogic.Services
{
    internal class OccupationService : BaseService, IOccupationService
    {
        private readonly IOccupationRepository _occupationRepository;
        private readonly AppDbContext _appContext;

        public OccupationService(IOccupationRepository occupationRepository, AppDbContext appDbContext) : base(appDbContext)
        {
            this._occupationRepository = occupationRepository;
        }
        public Task<object> Find(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> FindAll(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> FindAll(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable> GetAll()
        {
            var result = await this._context.TCOCCUPATIONS.Select(x => new { id = x.ID, description = x.DESCRIPTION }).ToListAsync();

            return result;
        }
    }
}
