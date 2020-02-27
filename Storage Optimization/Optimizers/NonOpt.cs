using StorageOptimization.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Optimizers
{
   public class NonOpt : Optimizer
    {
        private List<Order> orders;
        private List<ShipmentItem> shipment;

        public NonOpt(List<Order> orders, List<ShipmentItem> shipment)
        {
            this.orders = orders;
            this.shipment = shipment;
        }

        public List<Order> GetOpt()
        {
            List<Order> best_package = new List<Order>();

            foreach (Order order in orders)
            {
                if (CanDoneOrder(order, shipment))
                {
                    DoneOrder(order, best_package, shipment);
                }
            }
            return best_package;
        }
    }
}
