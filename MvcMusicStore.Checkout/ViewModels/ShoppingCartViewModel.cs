using System.Collections.Generic;
using MvcMusicStore.Checkout.Models;

namespace MvcMusicStore.Checkout.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}