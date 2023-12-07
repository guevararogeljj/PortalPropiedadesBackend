using DataSource.Models;
using System.Collections;

namespace DataSource.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        //T GetByIid(T entity);
        //IEnumerable GetAll();
        //void Add(T entity);
        //void Edit(T entity);
        //void Remove(T entity);
        //T GetIntance();


        public void Delete(T entity);

        public IEnumerable FindAll();

        public T FindById(T entity);

        public void Insert(T entity);
        public T InstanceObject();

        public List<T> InstanceList();

        public Saved Save();

        public void Update(T entity);
    }
}
