using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace ContactListApi.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region CONFIG
        private DbContext _context;//IAppDbContext
        private bool _shareContext;
        private bool _disposed;

        public bool ShareContext
        {
            get { return _shareContext; }
            set { _shareContext = value; }
        }

        public DbContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;

            }
        }

        public Repository(DbContext context)
        {
            Context = context;
            //_context.Database.CommandTimeout = 100000;
        }

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }


        #endregion

        #region Disposed

        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (ShareContext || _context == null) return;
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
            _disposed = true;
        }
        #endregion

        #region LINQ
        public IQueryable<TEntity> All()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }

        public IQueryable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(DbSet.AsNoTracking().AsQueryable().Where(predicate), (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
        }

        public TEntity FindInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty)).FirstOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public int Count
        {
            get { return DbSet.AsNoTracking().Count(); }
        }

        public int CountFunc(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Count(predicate);
        }

        public string MaxFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Where(where).AsNoTracking().Max(predicate);
        }

        public string Max(Expression<Func<TEntity, string>> predicate)
        {
            return DbSet.AsNoTracking().Max(predicate);
        }

        public string Min(Expression<Func<TEntity, string>> predicate)
        {
            return DbSet.Min(predicate);
        }

        public string MinFunc(Expression<Func<TEntity, string>> predicate, Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Where(where).Min(predicate);
        }

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public IQueryable<string> Select(Expression<Func<TEntity, string>> predicate)
        {
            return DbSet.Select(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().AsQueryable();
        }

        public virtual void Add(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            DbSet.Add(item);
            if (!ShareContext)
            {
                SaveChanges();
            }
        }

        public virtual void Modify(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
        }

        public TEntity Create(TEntity item)
        {
            DbSet.Add(item);
            if (!ShareContext)
            {
                SaveChanges();
            }
            return item;
        }

        public int Update(TEntity item)
        {
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return !ShareContext ? SaveChanges() : 0;
        }

        public TEntity CreateOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity newItem)
        {
            var record = DbSet.FirstOrDefault(predicate);
            if (record == null)
            {
                DbSet.Add(newItem);
            }
            else
            {
                var entry = _context.Entry(record);
                entry.CurrentValues.SetValues(newItem);
            }
            var result = !ShareContext ? SaveChanges() : 0;
            return result > 0 ? newItem : null;
        }
        public TEntity CreateOrUpdate(TEntity item)
        {
            var pi = item.GetType().GetProperty("Id");
            var keyFieldId = pi != null ? pi.GetValue(item, null) : 0;

            var record = DbSet.Find(keyFieldId);
            if (record == null)
            {
                DbSet.Add(item);
            }
            else
            {
                _context.Entry(record).CurrentValues.SetValues(item);
            }

            var result = !ShareContext ? SaveChanges() : 0;
            return result > 0 ? item : null;
        }

        public int Update(Expression<Func<TEntity, bool>> predicate)
        {
            var records = Where(predicate);
            if (!records.Any())
            {
                throw new NullReferenceException();
            }
            foreach (var record in records)
            {
                var entry = _context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return !ShareContext ? SaveChanges() : 0;
        }

        public int Delete(TEntity item)
        {
            DbSet.Attach(item);
            DbSet.Remove(item);

            return !ShareContext ? SaveChanges() : 0;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var records = FindAll(predicate);

            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return !ShareContext ? SaveChanges() : 0;
        }

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }

        public List<TEntity> CreateList(List<TEntity> items)
        {
            DbSet.AddRange(items);
            SaveChanges();
            return items;
        }
        public List<TEntity> UpdateList(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            try
            {
                foreach (var item in items)
                {
                    var entry = _context.Entry(item);
                    DbSet.Attach(item);
                    entry.State = EntityState.Modified;
                }
                var result = SaveChanges();
                return result > 0 ? items : null;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }
        #endregion

        #region SaveChange
        public int SaveChanges()
        {
            try
            {
                var ignorClass = typeof(TEntity).IsDefined(typeof(Attribute), false);
                int rowAffect = 0;

                using (var scope = new TransactionScope())
                {
                    rowAffect = _context.SaveChanges();
                    scope.Complete();
                }
                return rowAffect;
            }
            catch (Exception ex)
            {
               // ex.WriteLog();
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}