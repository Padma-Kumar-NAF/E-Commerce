using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Domain.Entities
{
    public class ProductStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
