using System.Collections.Generic;

namespace DesignPatterns.Prototype
{
    public class Product : ICloneable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class Order : ICloneable
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; }

        public object Clone()
        {
            var clonedOrder = (Order)MemberwiseClone();
            clonedOrder.Products = new List<Product>();

            foreach (var product in Products)
            {
                clonedOrder.Products.Add((Product)product.Clone());
            }

            return clonedOrder;
        }
    }
}
