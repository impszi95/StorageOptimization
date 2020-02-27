using StorageOptimization.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Optimizers
{
    public class Moho
    {
        private List<Order> orders;
        private List<ShipmentItem> shipment;

        List<Order> opt_orders = new List<Order>();
        public List<Order> Opt_orders { get => opt_orders; set => opt_orders = value; }

        public Moho(List<Order> orderss, List<ShipmentItem> shipments)
        {
            this.orders = new List<Order>(orderss);
            this.shipment = new List<ShipmentItem>(shipments);
        }
        
       public List<Order> Opt()
        {
            SortByQuantity();

            foreach (Order order in orders)
            {
                if (CanDoneOrder(order))
                {
                    DoneOrder(order);
                }
            }
            return opt_orders;
        }

        private void DoneOrder(Order order)
        {           
            opt_orders.Add(order);
            foreach (OrderItem item in order.OrderItems)
            {
                var idx = shipment.IndexOf(shipment.Where(x => x.ItemName == item.ItemName).FirstOrDefault());
                shipment[idx].Quantity -= item.Quantity;
            }
        }

        private void SortByQuantity()
        {
            orders = orders.OrderByDescending(x => x.TotalItems).ToList();
        }
        
        private bool CanDoneOrder(Order order)
        {
            foreach (OrderItem item in order.OrderItems)
            {
                if (!shipment.Any(x=>x.ItemName == item.ItemName))
                {
                    return false;
                }
                var qItem = shipment.Where(x => x.ItemName == item.ItemName).FirstOrDefault();
                
                var qQuantity = qItem.Quantity;

                if (qQuantity < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
