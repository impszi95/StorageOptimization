using StorageOptimization.Objects;
using StorageOptimization.Optimizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StorageOptimization.Optimizers
{
    public class Optimizer
    {
        private ObjectHandler handler;

        public Optimizer()
        {
            handler = new ObjectHandler();
        }

        public List<Order> GetMonteCarlo(List<Order> orders, List<ShipmentItem> shipment, CancellationToken token)
        {
            List<Order> input_orders = handler.CopyOrders(orders);
            List<ShipmentItem> orders_shipment = handler.CopyShipment(shipment);

            MonteCarlo mc = new MonteCarlo(input_orders, orders_shipment);
            return mc.GetOpt(token);
        }

        public List<Order> GetNonOpt(List<Order> orders, List<ShipmentItem> shipment)
        {
            List<Order> input_orders = handler.CopyOrders(orders);
            List<ShipmentItem> orders_shipment = handler.CopyShipment(shipment);

            NonOpt nonOpt = new NonOpt(input_orders, orders_shipment);
            return nonOpt.GetOpt();
        }
        virtual public void DoneOrder(Order order,List<Order> package, List<ShipmentItem> shipment)
        {
            package.Add(order);
            foreach (OrderItem orderItem in order.OrderItems)
            {
                var idx = shipment.IndexOf(shipment.Where(x => x.ItemName == orderItem.ItemName).FirstOrDefault());
                shipment[idx].Quantity -= orderItem.Quantity;
            }
        }
        virtual public void RemoveOrder(int orderID, List<Order> package, List<ShipmentItem> shipment)
        {
            Order to_remove = package.Where(x => x.OrderID == orderID).FirstOrDefault();

            foreach (OrderItem orderItem in to_remove.OrderItems)
            {
                var idx = shipment.IndexOf(shipment.Where(x => x.ItemName == orderItem.ItemName).FirstOrDefault());
                shipment[idx].Quantity += orderItem.Quantity;
            }
            package.Remove(to_remove);
        }
        virtual public bool CanDoneOrder(Order order, List<ShipmentItem> shipment)
        {
            foreach (OrderItem item in order.OrderItems)
            {
                if (!shipment.Any(x => x.ItemName == item.ItemName))
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