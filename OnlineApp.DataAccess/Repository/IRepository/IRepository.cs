using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineApp.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T - Category

        // To get all elements of the class for listing
        IEnumerable<T> GetAll();

        // To get single value based on id or any element, it will return first or default
        // The param is example how LINQ value is passed as generic param
        T Get(Expression<Func<T, bool>> filter);

        // Adding an entity to the db for the class T
        void Add(T entity);

        // Deleting an entity to the db for the class T
        void Remove(T entity);

        // Deleting a range of entities from a column
        void RemoveRange(IEnumerable<T> entity);
    }
}
