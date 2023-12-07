using BusinessLogic.Contracts;
using DataSource;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class CitiesService : BaseService, ICitiesService
    {
        public CitiesService(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<object> Find(int id)
        {
            var city = await this._context.TCCITIES.Where(x => x.ID == id).Select(y => new { id = y.ID, description = y.DESCRIPTION }).FirstOrDefaultAsync();

            return city;
        }

        public async Task<IEnumerable> FindAll(int id)
        {
            var cities = await this._context.TCCITIES.Where(x => x.CODESTATENavigation.ID == id && x.TADDRESSES.Any(o => o.IDPROPERTYNavigation.IDSTATUS == 1)).Select(y => new { id = y.ID, description = y.DESCRIPTION }).ToListAsync();

            return cities;
        }

        public async Task<IEnumerable> FindAll(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable> GetAll()
        {
            var cities = await this._context.TCCITIES.Select(y => new { id = y.ID, description = _textInfo.ToTitleCase(y.DESCRIPTION.ToLower()) }).ToListAsync();

            return cities;
        }
    }
}
