using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Warehouses
{
    public class UpdatePriceRequest
    {
        public int Id { get; set; }

        public decimal NewPrice { set; get; }

        public decimal NewOriginalPrice { set; get; }
    }
}
