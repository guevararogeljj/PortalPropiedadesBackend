using BusinessLogic.Contracts;
using DataSource;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BusinessLogic.Services
{
    internal class PropertyTypeService : BaseService, IPropertyTypeService
    {
        public PropertyTypeService(AppDbContext context) : base(context)
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
        /// Get all records from properties type catalog
        /// </summary>
        /// <returns>Object list with properties(id, description)</returns>
        public async Task<IEnumerable> GetAll()
        {
            return await this._context.TCTYPE.Where(x => x.IDSTATUS == 1).Select(x => new { id = x.ID, description = _textInfo.ToTitleCase(x.DESCRIPTION.ToLower()) }).ToListAsync();
        }
    }
}
