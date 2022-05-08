using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Warehouses
{
    public class UpdateStockRequest
    {
        public int Id { get; set; }

        public int Quantity { set; get; }
    }
}
