using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StorageOptimization.Factories;
using StorageOptimization.Generator;
using StorageOptimization.Optimizers;

namespace UnitTests
{
    [TestFixture]
    public class ObjectGeneratorTests
    {
        private Optimizer optimizer;

        [TestCase(1,1)]
        [TestCase(1, 20)]
        [TestCase(30, 1)]
        [TestCase(20,20)]
        [TestCase(123,44)]
        public void WhenGenerateObjects_OrdersNumberCorrect(int items_number,int orders_number)
        {
            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);
            ObjectGenerator obj_generator = new ObjectGenerator();

            csv_generator.GenerateOrdersFile();

            var orders = obj_generator.CreateOrders(); 
            var orderId_num = orders.SelectMany(x => x.OrderItems).GroupBy(y => y.OrderID).Count();

            Assert.That(orderId_num == orders_number);
        }

        [TestCase(1, 1)]
        [TestCase(1, 20)]
        [TestCase(30, 1)]
        [TestCase(20, 20)]
        [TestCase(123, 44)]
        public void WhenGenerateObjects_NoRepeatedItemInOrder(int items_number, int orders_number)
        {
            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);
            ObjectGenerator obj_generator = new ObjectGenerator();

            csv_generator.GenerateOrdersFile();

            var orders = obj_generator.CreateOrders();
            var order_by_id = orders.GroupBy(y => y.OrderId);

            foreach (var order in order_by_id)
            {
                var num = order.SelectMany(x => x.OrderItems).GroupBy(y => y.ItemName).Count();
                Assert.That(num <= items_number);
            }
        }

        [TestCase(1, 8)]
        [TestCase(10, 20)]
        [TestCase(50, 50)]
        [TestCase(50, 80)]
        [TestCase(70, 35)]
        [TestCase(16, 28)]
        public void OptimizationStoresLessItems_SmallerAmounts(int items_number, int orders_number)
        {
            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);
            ObjectGenerator obj_generator = new ObjectGenerator();
            optimizer = new Optimizer();
            CancellationTokenSource cts = new CancellationTokenSource();

            csv_generator.GenerateOrdersFile();
            var orders = obj_generator.CreateOrders();

            csv_generator.GenerateShipmentFile(orders);
            var shipment = obj_generator.CreateShipment();

            Random rnd = new Random();
            int test_num = 100;

            for (int i = 1; i <= test_num; i++)
            {
                var optimized_orders = optimizer.GetMonteCarlo(orders, shipment, cts.Token);
                var nonOpt_package = optimizer.GetNonOpt(orders, shipment);

                var opt_num = optimized_orders.Sum(x => x.TotalItems);
                var nonOpt_num = nonOpt_package.Sum(x => x.TotalItems);

                Assert.That(opt_num >= nonOpt_num);
            }            
        }

        [TestCase(100, 100)]
        [TestCase(100, 300)]
        [TestCase(300, 100)]
        public void OptimizationStoresLessItems_BiggerAmounts(int items_number, int orders_number)
        {
            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);
            ObjectGenerator obj_generator = new ObjectGenerator();
            optimizer = new Optimizer();
            CancellationTokenSource cts = new CancellationTokenSource();


            csv_generator.GenerateOrdersFile();
            var orders = obj_generator.CreateOrders();

            csv_generator.GenerateShipmentFile(orders);
            var shipment = obj_generator.CreateShipment();

            Random rnd = new Random();
            int test_num = 10;

            for (int i = 1; i <= test_num; i++)
            {
                var optimized_orders = optimizer.GetMonteCarlo(orders, shipment,cts.Token);
                var nonOpt_package = optimizer.GetNonOpt(orders, shipment);

                var opt_num = optimized_orders.Sum(x => x.TotalItems);
                var nonOpt_num = nonOpt_package.Sum(x => x.TotalItems);

                Assert.That(opt_num >= nonOpt_num);
            }
        }
    }
}
