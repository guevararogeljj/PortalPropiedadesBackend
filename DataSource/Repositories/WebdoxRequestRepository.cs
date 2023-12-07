using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class WebdoxRequestRepository : Repository<WEBDOXREQUEST>, IWebdoxRequestRepository
    {
        public WebdoxRequestRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public List<WEBDOXREQUEST> FindAllRequestByIdWebdox(string entity)
        {
            var result = this._context.WEBDOXREQUEST.Where(x => x.FORMWEBDOXID.Equals(entity)).ToList();

            return result;
        }

        public WEBDOXREQUEST FindRequestByIdWebdox(WEBDOXREQUEST entity)
        {
            var result = this._dbset.Where(x => x.FORMWEBDOXID.Equals(entity.FORMWEBDOXID)).FirstOrDefault();

            return result;
        }

        public override WEBDOXREQUEST FindById(WEBDOXREQUEST entity)
        {
            var result = this._dbset.Where(x => x.ID == entity.ID).FirstOrDefault();

            return result;
        }
    }
}
