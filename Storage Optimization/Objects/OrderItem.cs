using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization
{
  public class OrderItem
    {
        private int orderID;
        private int customerID;
        private string itemName;
        private int quantity;

        public int OrderID { get => orderID; set => orderID = value; }
        public int CustomerID { get => customerID; set => customerID = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
