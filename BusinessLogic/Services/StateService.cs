using BusinessLogic.Contracts;
using DataSource;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class StateService : BaseService, IStateService
    {
        public StateService(AppDbContext context) : base(context)
        {

        }

        public async Task<object> Find(int id)
        {
            var state = await this._context.TCSTATESv2.Where(x => x.ID == id).FirstOrDefaultAsync();

            return state;
        }

        public async Task<IEnumerable> FindAll(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable> FindAll(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable> GetAll()
        {
            var states = await this._context.TCSTATESv2.Where(x => x.TCCITIES.Any(y => y.TADDRESSES.Any(o => o.IDPROPERTYNavigation.IDSTATUS == 1))).Select(x => new { id = x.ID, description = x.DESCRIPTION }).ToListAsync();

            return states;
        }
    }
}
