using OnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineApp.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        // Update method for Product model
        void Update(Product obj);
    }
}
