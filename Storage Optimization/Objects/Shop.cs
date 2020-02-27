using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Objects
{
    public class Shop
    {
        private static Shop instance = null;

        private List<Order> orders;
        private List<ShipmentItem> shipment;
        private Dictionary<string, int> storage;

        public List<Order> Orders { get => orders; set => orders = value; }
        public List<ShipmentItem> Shipment { get => shipment; set => shipment = value; }
        public Dictionary<string, int> Storage { get => storage; set => storage = value; }

        private Shop()
        {
            orders = new List<Order>();
            shipment = new List<ShipmentItem>();
            storage = new Dictionary<string, int>();
        }
        public static Shop GetInstance()
        {
            if (instance == null)
            {
                instance = new Shop();
            }
            return instance;
        }
       
    }
}
