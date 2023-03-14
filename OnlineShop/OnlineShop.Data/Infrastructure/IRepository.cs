using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OnlineShop.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> Delete(T entity);

        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> Delete(int id);

        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        // Get an entity by int id
        T GetSingleById(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IEnumerable<T> GetAll(string[] includes = null);

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
    }
    //public interface IRepository<K> where K : class
    //{
    //    // Marks an entity as new
    //    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<K> Add(K entity);

    //    // Marks an entity as modified
    //    void Update(K entity);

    //    // Marks an entity to be removed
    //    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<K> Delete(K entity);

    //    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<K> Delete(int id);

    //    //Delete multi records
    //    void DeleteMulti(Expression<Func<K, bool>> where);

    //    // Get an entity by int id
    //    K GetSingleById(int id);

    //    K GetSingleByCondition(Expression<Func<K, bool>> expression, string[] includes = null);

    //    IEnumerable<K> GetAll(string[] includes = null);

    //    IEnumerable<K> GetMulti(Expression<Func<K, bool>> predicate, string[] includes = null);

    //    IEnumerable<K> GetMultiPaging(Expression<Func<K, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

    //    int Count(Expression<Func<K, bool>> where);

    //    bool CheckContains(Expression<Func<K, bool>> predicate);
    //}
}
