using BusinessLogic.Contracts;
using DataSource;
using DataSource.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections;


namespace BusinessLogic.Services
{
    internal class MaritalstatusService : BaseService, IMaritalstatusService
    {
        private readonly IMaritalstatusRepository _maritalstatusRepository;
        private readonly AppDbContext _appContext;

        public MaritalstatusService(IMaritalstatusRepository maritalstatusRepository, AppDbContext appDbContext) : base(appDbContext)
        {
            this._maritalstatusRepository = maritalstatusRepository;
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
            var result = await this._context.TCMARITALSTATUS.Select(x => new { id = x.ID, description = x.DESCRIPTION }).ToListAsync();

            return result;
        }
    }
}
