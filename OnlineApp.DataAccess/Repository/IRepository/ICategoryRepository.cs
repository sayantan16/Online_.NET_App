using OnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineApp.DataAccess.Repository.IRepository
{
    // Class/Model specific interface which implements the generic repository
    // Class/Model name is specified as when this interface is implemented, the class can use the model to add data access functions
    public interface ICategoryRepository : IRepository<Category>
    {
        // Update method for Category model
        void Update(Category obj);

        // _db.SaveChanges() related functionality to persist changes done in the Categories table
        void Save();
    }
}
