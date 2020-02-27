using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization
{
  public class OrderItem
    {
        private int orderId;
        private int customerId;
        private string itemName;
        private int quantity;

        public int OrderId { get => orderId; set => orderId = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
