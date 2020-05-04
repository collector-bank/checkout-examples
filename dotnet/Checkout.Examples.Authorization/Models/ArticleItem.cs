namespace Checkout.Examples.Authorization.Models
{
    public class ArticleItem
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Vat { get; set; }

        public int Quantity { get; set; }
    }
}
