using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineApp.DataAccess.Data;
using OnlineApp.DataAccess.Repository.IRepository;

namespace OnlineApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            // dbSet can add the entity object in the class of type T in DB
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            // query holds all values from DB for class T
            IQueryable<T> query = dbSet;

            // on the query object filter is applied as a LINQ query
            query = query.Where(filter);

            // the first of default value from the filtered query is returned
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            // Returning the whole list of values from the DB as query holds all values from DB for class T
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            // Removing the entity from the dbset in class T
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            // Remove the range of entities from the dbset on class T
            dbSet.RemoveRange(entity);
        }
    }
}
