using DataSource.Contracts;
using DataSource.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories
{
    internal class PropertyRepository : Repository<TPROPERTIES>, IPropertyRepository
    {
        public PropertyRepository(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public TPROPERTIES FindPropertyByCreditNumber(string creditnumber)
        {
            var property = this._dbset.Where(x => x.CREDITNUMBER.Equals(creditnumber)).FirstOrDefault();

            return property;
        }

        public Dictionary<string, object> SharePropertyInfo(string creditnumber)
        {
            var property = this._context.TPROPERTIES.Include(x => x.TFILES).Where(x => x.ID.ToString().Equals(creditnumber)).FirstOrDefault();

            var dic = new Dictionary<string, object>();
            if (property != null)
            {
                var image = property.TFILES.Where(x => x.PREVIEW == 0).FirstOrDefault();

                dic.Add("Price", property.SALEPRICE);
                dic.Add("Description", property.DESCRIPTION);
                dic.Add("ImagePath", image.PATH);
                dic.Add("ImageExt", image.FILEEXTENCION);
                dic.Add("ImageName", image.TITLE);
                dic.Add("Credit", property.CREDITNUMBER);
                dic.Add("Title", property.TITLE);

            }

            return dic;
        }

        public override TPROPERTIES FindById(TPROPERTIES entity)
        {
            return this._dbset.Where(x => x.ID == entity.ID).FirstOrDefault();
        }

        public IEnumerable<TPROPERTIES> AvailableProperties()
        {


            return this._dbset
            .Include(x => x.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation)
            .Include(x => x.TFILES)
            .Include(x => x.IDTYPENavigation)
            .Include(x => x.IDBEDROOMNavigation)
            .Include(x => x.IDPARKINGSPACENavigation)
            .Include(x => x.IDHALFBATHROOMNavigation)
            .Include(x => x.IDBATHROOMNavigation)
            .Where(x => x.IDSTATUS == 1 &&
            x.TFILES.Any(y => y.PREVIEW == 1 && y.IDSTATUS == 1) &&
            x.TFILES.Any(z => z.PREVIEW == 0 && z.IDSTATUS == 1));
        }

        public IEnumerable<TPROPERTIES> PageSizeAvailableProperties(int? index, int? pagesize, ExpressionStarter<TPROPERTIES> where = null)
        {
            if (where == null)
            {
                return this.AvailableProperties().Skip((index - 1 ?? 0) * pagesize.GetValueOrDefault()).Take(pagesize.GetValueOrDefault()).ToList();

            }

            return this.AvailableProperties().Where(where).Skip((index - 1 ?? 0) * pagesize.GetValueOrDefault()).Take(pagesize.GetValueOrDefault()).ToList();
        }

        public int? CountAvailableProperties(int? index, int? pagesize, ExpressionStarter<TPROPERTIES> where = null)
        {
            if (where == null)
            {
                return this.AvailableProperties().ToList().Count;

            }

            return this.AvailableProperties().Where(where).ToList().Count;
        }

    }
}
