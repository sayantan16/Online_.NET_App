using OnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineApp.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        // Update method for Order Header model
        void Update(OrderHeader obj);

        // based on id, we want to only update order status or payment status
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        // based on order header id, updating the session and payment id for stripe payments
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    }
}
