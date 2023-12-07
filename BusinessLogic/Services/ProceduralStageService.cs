using BusinessLogic.Contracts;
using DataSource;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class ProceduralStageService : BaseService, IProceduralStageService
    {

        public ProceduralStageService(AppDbContext appDbContext) : base(appDbContext)
        {

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

        /// <summary>
        /// Get all records from procedural stage catalog
        /// </summary>
        /// <returns>Object list with properties (id,description)</returns>
        public async Task<IEnumerable> GetAll()
        {
            var stages = await this._context.TCPROCEDURALSTAGE.Select(x => new { id = x.ID, description = _textInfo.ToTitleCase(x.DESCRIPTION.ToLower()) }).ToListAsync();

            return stages;
        }
    }
}
