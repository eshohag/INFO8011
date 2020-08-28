using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContactListApi.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        #region CONFIG
        DbContext Context { get; set; }
        bool ShareContext { get; set; }
        #endregion

        #region LINQ
        IQueryable<TEntity> All();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        IQueryable<string> Select(Expression<Func<TEntity, string>> predicate);
        TEntity Find(params object[] keys);
        IQueryable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity FindInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        bool IsExist(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        int Count { get; }
        int CountFunc(Expression<Func<TEntity, bool>> predicate);
        string MaxFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        string Max(Expression<Func<TEntity, string>> predicate);
        string Min(Expression<Func<TEntity, string>> predicate);
        string MinFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where);
        void Add(TEntity item);
        void Modify(TEntity item);
        TEntity Create(TEntity item);
        int Update(TEntity item);
        int Delete(TEntity item);
        TEntity CreateOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity newItem);
        TEntity CreateOrUpdate(TEntity item);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion
    }
}