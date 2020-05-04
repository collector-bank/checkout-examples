namespace Checkout.Examples.Authorization.Models
{
    public class FeeItem
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Vat { get; set; }
    }
}
