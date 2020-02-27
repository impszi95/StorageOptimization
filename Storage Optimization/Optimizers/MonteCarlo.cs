using StorageOptimization.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StorageOptimization.Optimizers
{
    public class MonteCarlo : Optimizer
    {
       private List<Order> orders;
       private List<ShipmentItem> shipment;
   
       private List<Order> all_best;
       private List<Order> actual_best;
       private List<Order> actual_package;
        
       private List<ShipmentItem> all_best_shipment;
       private List<ShipmentItem> actual_best_shipment;
       private List<ShipmentItem> actual_package_shipment;
      
       private Random rnd;
       private ObjectHandler handler;

       private int actual_best_db;
       private List<int> result_list;
       private int slide_w_size;

        public MonteCarlo(List<Order> orders, List<ShipmentItem> shipment)
        {
            #region Inits
                        
            this.orders = orders;
            this.shipment = shipment;

            all_best = new List<Order>();
            actual_best = new List<Order>();
            actual_package = new List<Order>();

            all_best_shipment = new List<ShipmentItem>();
            actual_best_shipment = new List<ShipmentItem>();
            actual_package_shipment = new List<ShipmentItem>();

            rnd = new Random();
            handler = new ObjectHandler();

            actual_best_db = 0;
            result_list = new List<int>();
            #endregion

            Moho moho = new Moho(orders, shipment);
            List<Order> moho_opt = handler.CopyOrders(moho.Opt());
            
            Console.WriteLine("Moho: " + moho_opt.Sum(x => x.TotalItems));
            all_best = handler.CopyOrders(moho_opt);
            actual_best = handler.CopyOrders(moho_opt);
            actual_package = new List<Order>();

            all_best_shipment = shipment;
            actual_best_shipment = shipment;
            actual_package_shipment = shipment;

            slide_w_size = 50;
        }

        public List<Order> GetOpt(CancellationToken token)
        {            
            int i = 0;
            while (!IsLanded() && !token.IsCancellationRequested)
            {
                Shake();

                LogActualResult(i);
                
                i++;
            }
            return actual_best;
        }

        public void Shake()
        {
            RandomPickOut();

            List<Order> temp_oders = handler.CopyOrders(orders);
            handler.Shuffle(temp_oders);

            foreach (Order order in temp_oders)
            {
                if (CanDoneOrder(order, actual_package_shipment))
                {
                    DoneOrder(order,actual_package,actual_package_shipment);
                }
            }
            actual_best_db = actual_best.Sum(x => x.TotalItems);
            int actual_pakolas_db = actual_package.Sum(x => x.TotalItems);
            if (actual_best_db <= actual_pakolas_db)
            {
                actual_best_db = actual_pakolas_db;
                actual_best = handler.CopyOrders(actual_package);
                actual_best_shipment = handler.CopyShipment(actual_package_shipment);
            }
            else //Previus state was better (stepback)
            {
                actual_package = handler.CopyOrders(actual_best);
                actual_package_shipment = handler.CopyShipment(actual_best_shipment);
            }
        }

        private void LogActualResult(int i)
        {
            Console.WriteLine(i + ". " + actual_best_db);
        }

        private bool IsLanded()
        {
            result_list.Add(actual_best_db);

            if (result_list.Count > slide_w_size)
            {
                var slide_window = result_list.GetRange(result_list.Count - (slide_w_size + 1), slide_w_size);
                if (slide_window.All(x => x == slide_window[0]))
                {
                    return true;
                }
            }
            return false;
        }

        private void RandomPickOut()
        {
            actual_package = handler.CopyOrders(actual_best);
            List<Order> temp_package = handler.CopyOrders(actual_package);

            foreach (Order order in actual_package)
            {                 
                if (rnd.NextDouble() < 0.1)
                {
                    //Put back to storage
                    RemoveOrder(order.OrderId, temp_package, actual_package_shipment);
                }
            }
            actual_package = handler.CopyOrders(temp_package);
        }
    }
}
