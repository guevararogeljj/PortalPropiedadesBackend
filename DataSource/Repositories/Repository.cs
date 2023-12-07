using DataSource.Interfaces;
using DataSource.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataSource.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbset;

        public Repository(DbContextOptions<AppDbContext> options)
        {
            this._context = new AppDbContext(options);
            this._dbset = _context.Set<T>();
        }


        //protected readonly AppDbContext _context;

        //public Repository(DbContextOptions<AppDbContext> options)
        //{
        //    this._context = new AppDbContext(options);
        //}

        public void Delete(T entity)
        {
            this._dbset.Remove(entity);
        }

        public IEnumerable FindAll()
        {
            return this._dbset.ToList();
        }

        public virtual T FindById(T entity)
        {
            return this._dbset.Find(entity);
        }

        public void Insert(T entity)
        {
            this._dbset.Add(entity);
        }

        public List<T> InstanceList()
        {
            return new List<T>();
        }

        public T InstanceObject()
        {
            return new T();
        }

        public Saved Save()
        {
            try
            {
                this._context.SaveChanges();
                return new Saved(true);

            }
            catch (Exception ex)
            {

                return new Saved(false, ex.ToString());
            }

        }

        public void Update(T entity)
        {
            this._dbset.Update(entity);
        }

    }
}
