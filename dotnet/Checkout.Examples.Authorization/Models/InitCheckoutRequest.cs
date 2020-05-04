namespace Checkout.Examples.Authorization.Models
{
    public class InitCheckoutRequest
    {
        public InitCheckoutRequest()
        {
            Cart = new CartModel();
        }

        public int StoreId { get; set; }

        public string CountryCode { get; set; }

        public string ProfileName { get; set; }

        public string Reference { get; set; }

        public string NotificationUri { get; set; }

        public string RedirectPageUri { get; set; }

        public string MerchantTermsUri { get; set; }

        public object Metadata { get; set; }

        public CartModel Cart { get; set; }

        public FeesModel Fees { get; set; }

        public CustomerModel Customer { get; set; }
    }
}