using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Sales
{
    public class OrderVM
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipPhoneNumber { get; set; }

        public OrderStatus Status { get; set; }
    }
}
