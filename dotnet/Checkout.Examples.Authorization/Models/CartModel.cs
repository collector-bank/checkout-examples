using System.Collections.Generic;

namespace Checkout.Examples.Authorization.Models
{
    public class CartModel
    {
        public CartModel()
        {
            Items = new List<ArticleItem>();
        }

        public List<ArticleItem> Items { get; set; }
    }
}