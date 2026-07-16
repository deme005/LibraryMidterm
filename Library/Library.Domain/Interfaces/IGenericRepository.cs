using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        List<T> GetAll();
        T Get (Func<T,bool> predicate);
        T GetByKey(string key);
        void Delete(T entity);
        void Update(T entity);
        void SaveChanges(List<T> entities);
    }
}
