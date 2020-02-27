using StorageOptimization.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOptimization.Factories
{
    public class CsvGenerator
    {
        private int items_number;
        private int orders_number;
        private StringBuilder sb;
        private char determiner;

        public CsvGenerator(int items_number, int orders_number)
        {
            this.items_number = items_number;
            this.orders_number = orders_number;
            determiner = ',';
        }

        public void GenerateOrdersFile()
        {
            sb = new StringBuilder();
            Random rnd = new Random();
            sb.AppendLine("OrderId" + determiner + "CustomerId" + determiner + "ItemName" + determiner +"Quantity");

            for (int i = 1; i <= orders_number; i++)
            {
                List<int> already_ordered_items = new List<int>();

                int itemsMax = 5; //Itt állítható items/order max
                int itemsPerOrder = rnd.Next(1, itemsMax + 1);
                if (items_number <= itemsMax) //Csak annyi db terméket lehessen rendelni 1 orderben max, amennyi fajta termék van. (ism. miatt)
                {
                    itemsPerOrder = rnd.Next(1, items_number + 1);
                }

                for (int j = 1; j <= itemsPerOrder; j++)
                {
                    var orderID = i;
                    sb.Append(orderID);
                    sb.Append(determiner);

                    var customerID = i;
                    sb.Append(customerID);
                    sb.Append(determiner);

                    int product_id = rnd.Next(1, items_number + 1); //Product duplikálás ne legyen orderen belül.
                    while (already_ordered_items.Contains(product_id))
                    {
                        product_id = rnd.Next(1, items_number + 1);
                    }
                    already_ordered_items.Add(product_id);

                    var itemName = "Product_" + product_id; //Nem biztosítom hogy, az öszes termékből lesz rendelés.
                    sb.Append(itemName);
                    sb.Append(determiner);

                    var quantity = rnd.Next(1, 5);
                    sb.Append(quantity);

                    sb.AppendLine();
                }
            }
            string orders_path = "customer_orders.csv";
            if (File.Exists(orders_path))
            {
                File.Delete(orders_path);
            }
            File.WriteAllText(orders_path, sb.ToString(), Encoding.Default);
        }

        public void GenerateShipmentFile(List<Order> orders)
        {

            sb = new StringBuilder();
            Random rnd = new Random();
            sb.AppendLine("ItemName"+determiner+"Quantity");
            

            int number_of_items = orders.SelectMany(x=>x.OrderItems.ToList()).GroupBy(y => y.ItemName).Count();
            var orders_by_items = orders.SelectMany(x=>x.OrderItems).ToList().GroupBy(x => x.ItemName).ToList();

            for (int i = 0; i < number_of_items; i++) //I want the factory to send min. 1 piece from every item
            {
                var itemName = orders_by_items[i].Key;
                sb.Append(itemName);
                sb.Append(determiner);

                int max_from_product = orders_by_items[i].Sum(x => x.Quantity); //Send max the amount that has been ordered (dont send extra ones)
                double factory_capacity = 0.5;
                int min_from_product = (int)Math.Floor(max_from_product * factory_capacity);
                int quantity = rnd.Next(min_from_product, max_from_product + 1); //Configure of items/shipment 
                sb.Append(quantity);

                sb.AppendLine();
            }

            string orders_path = "factory_shipment.csv";
            if (File.Exists(orders_path))
            {
                File.Delete(orders_path);
            }
            File.WriteAllText(orders_path, sb.ToString(), Encoding.Default);
        }
    }
}
