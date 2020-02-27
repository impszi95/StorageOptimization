using StorageOptimization.Factories;
using StorageOptimization.Generator;
using StorageOptimization.Objects;
using StorageOptimization.Optimizers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StorageOptimization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shop shop;
        private ObjectGenerator obj_generator;
        private Optimizer optimizer;
        private CancellationTokenSource cts;
        private int limit;

        public MainWindow()
        {
            InitializeComponent();

            shop = Shop.GetInstance();
            obj_generator = new ObjectGenerator();
            optimizer = new Optimizer();
            limit = 0;
            cts = new CancellationTokenSource();
        }

        private void Generate_csv_button_Click(object sender, RoutedEventArgs e)
        {
            int items_number = (int)items_slider.Value;
            int orders_number = (int)orders_slider.Value;

            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);

            csv_generator.GenerateOrdersFile();
            shop.Orders = obj_generator.CreateOrders(); // 1.Input Csv
            csv_generator.GenerateShipmentFile(shop.Orders);  //Send Sum to Factory
            shop.Shipment = obj_generator.CreateShipment();  // 2.Input Csv

            all_label.Content = shop.Orders.Sum(x => x.TotalItems);
        }

        private void Optimize_button_Click(object sender, RoutedEventArgs e)
        {
            List<Order> nonOpt_package = optimizer.GetNonOpt(shop.Orders, shop.Shipment);
            List<Order> optimized_orders = new List<Order>();
            
            if (checkBox_timeLimit.IsChecked == true)
            {
                bool time_limit_empty = String.IsNullOrEmpty(textBox_timeLimit.Text);
                if (time_limit_empty)
                {
                    MessageBox.Show("Missed time limit!");
                    return;
                }
                limit = int.Parse(textBox_timeLimit.Text);
                cts = new CancellationTokenSource();

                Task.Run(() => CheckTimeLimit());
                optimized_orders = OptimizeWithTimeLimit();
            }
            else
            {
                optimized_orders = OptimizeNoTimeLimit();
            }

            opt_label.Content = optimized_orders.Sum(x => x.TotalItems);
            non_opt_label.Content = nonOpt_package.Sum(x => x.TotalItems);
        }

        private List<Order> OptimizeWithTimeLimit()
        {
            return Task.Run(() => optimizer.GetMonteCarlo(shop.Orders, shop.Shipment, cts.Token)).Result;
        }

        private List<Order> OptimizeNoTimeLimit()
        {
            return optimizer.GetMonteCarlo(shop.Orders, shop.Shipment, cts.Token);
        }

        private void CheckTimeLimit()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed.Seconds < limit)
            {
            }
            cts.Cancel();
            sw.Reset();
        }

        private void Read_input_buttonClick_Click(object sender, RoutedEventArgs e)
        {
            shop.Orders = obj_generator.CreateOrders();
            shop.Shipment = obj_generator.CreateShipment();
            all_label.Content = shop.Orders.Sum(x => x.TotalItems);
        }
    }
}
