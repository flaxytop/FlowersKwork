using Flowers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowers.Domain.Cart
{
    public class CartItem
    {
        public FN Item { get; set; }
        public int Qty { get; set; }

    }
}
