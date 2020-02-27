using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using StorageOptimization.Objects;

namespace StorageOptimization.Generator
{
    public class ObjectGenerator
    {

        private List<OrderItem> CreateOrderItems()
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.Delimiter = ";";
            using (var reader = new StreamReader("customer_orders.csv"))           
            using (var csv = new CsvReader(reader,config))
            {
               return csv.GetRecords<OrderItem>().ToList();
            }
        }

        public List<Order> CreateOrders()
        {
            List<Order> orders = new List<Order>();
            var orderItems_by_id = CreateOrderItems().GroupBy(x => x.OrderID).ToList();
            foreach (var order in orderItems_by_id)
            {
                orders.Add(new Order(){ OrderID = order.Key, OrderItems = order.ToList() });
            }
            return orders;
        }

        public List<ShipmentItem> CreateShipment()
        {            
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.Delimiter = ";";
            using (var reader = new StreamReader("factory_shipment.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<ShipmentItem>().ToList();
            }
        }
    }
}
