﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Sales
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; }

        public CheckoutRequest CheckoutModel { get; set; }
    }
}
