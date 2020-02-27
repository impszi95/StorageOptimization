using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Objects
{
   public class Order
    {
        private int orderId;
        private List<OrderItem> orderItems;

        public List<OrderItem> OrderItems { get => orderItems; set => orderItems = value; }
        public int OrderId { get => orderId; set => orderId = value; }
        public int TotalItems { get => orderItems.Sum(x=>x.Quantity); }
    }
}
