using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository.IRepository;

namespace Youssef.DataAccess.Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _Db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext Db)
        {
            _Db = Db;
            this.dbSet = _Db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();

        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
