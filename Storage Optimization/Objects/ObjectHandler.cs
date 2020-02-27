using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Objects
{
   public class ObjectHandler
    {
        public List<ShipmentItem> CopyShipment(List<ShipmentItem> shipment)
        {
            List<ShipmentItem> temp_shipment = new List<ShipmentItem>();
            shipment.ForEach(x => temp_shipment.Add(new ShipmentItem() { ItemName = x.ItemName, Quantity = x.Quantity }));
            return temp_shipment;
        }

        public List<Order> CopyOrders(List<Order> orders)
        {
            List<Order> copied_orders = new List<Order>();
            foreach (Order order in orders)
            {
                Order copied_order = CopyOrder(order);
                copied_orders.Add(copied_order);
            }
            return copied_orders;
        }

        public Order CopyOrder(Order order)
        {
            List<OrderItem> copied_orderItems = new List<OrderItem>();
            order.OrderItems.ForEach(
                x => copied_orderItems.Add(new OrderItem()
                {
                    CustomerId = x.CustomerId,
                    ItemName = x.ItemName,
                    OrderId = x.OrderId,
                    Quantity = x.Quantity
                }));

            Order copied_order = new Order()
            {
                OrderId = order.OrderId,
                OrderItems = copied_orderItems
            };
            return copied_order;
        }

        public void Shuffle(List<Order> list)
        {
            Random rnd = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Order value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
