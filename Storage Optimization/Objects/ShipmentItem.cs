using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization
{
   public class ShipmentItem
    {
        private string itemName;
        private int quantity;

        public string ItemName { get => itemName; set => itemName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
