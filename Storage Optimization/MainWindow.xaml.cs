using StorageOptimization.Factories;
using StorageOptimization.Generator;
using StorageOptimization.Objects;
using StorageOptimization.Optimizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace StorageOptimization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shop shop;
        private Optimizer optimizer;
        private ObjectHandler handler;

        public MainWindow()
        {
            InitializeComponent();

            shop = Shop.GetInstance();
            optimizer = new Optimizer();
        }

        private void Generate_csv_button_Click(object sender, RoutedEventArgs e)
        {
            int items_number = (int)items_slider.Value;
            int orders_number = (int)orders_slider.Value;

            CsvGenerator csv_generator = new CsvGenerator(items_number, orders_number);
            ObjectGenerator obj_generator = new ObjectGenerator();

            csv_generator.GenerateOrdersFile(); 

            shop.Orders = obj_generator.CreateOrders(); // 1.Input Csv

            csv_generator.GenerateShipmentFile(shop.Orders);  //Send Sum to Factory

            shop.Shipment = obj_generator.CreateShipment();  // 2.Input Csv

            all_label.Content = shop.Orders.Sum(x => x.TotalItems);
        }

        private void Optimize_button_Click(object sender, RoutedEventArgs e)
        {           
            List<Order> optimized_orders = optimizer.GetMonteCarlo(shop.Orders, shop.Shipment);            
            List<Order> nonOpt_package = optimizer.GetNonOpt(shop.Orders, shop.Shipment);
            
            opt_label.Content = optimized_orders.Sum(x => x.TotalItems);
            non_opt_label.Content = nonOpt_package.Sum(x => x.TotalItems);
        }
    }
}
