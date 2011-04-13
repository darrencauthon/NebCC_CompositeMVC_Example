using System;
using System.Linq;
using System.Web;
using MvcMusicStore.Checkout.Models;

namespace MvcMusicStore.Checkout.Helpers
{
    public interface ICartRetriever
    {
        ShoppingCart GetTheCurrentCart();
    }

    public interface ICartIdRetriever
    {
        string GetTheCartId();
    }

    public interface ICartMigrator
    {
        void MigrateCart(string username);
    }

    public class CartIdRetriever : ICartIdRetriever
    {
        private const string CartSessionKey = "CartSessionKey";

        public string GetTheCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    var tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return HttpContext.Current.Session[CartSessionKey].ToString();
        }
    }

    public class CartMigrator : ICartMigrator
    {
        private readonly CheckoutEntities checkoutEntities;
        private readonly ICartIdRetriever cartIdRetriever;

        public CartMigrator(ICartIdRetriever cartIdRetriever,
                            CheckoutEntities checkoutEntities)
        {
            this.cartIdRetriever = cartIdRetriever;
            this.checkoutEntities = checkoutEntities;
        }

        public void MigrateCart(string username)
        {
            // Associate shopping cart items with logged-in user
            var shoppingCart = checkoutEntities.Carts.Where(c => c.CartId == cartIdRetriever.GetTheCartId());
            foreach (var item in shoppingCart)
            {
                item.CartId = username;
            }
            checkoutEntities.SaveChanges();

            HttpContext.Current.Session[ShoppingCart.CartSessionKey] = username;
        }
    }

    public class CartRetriever : ICartRetriever
    {
        private readonly CheckoutEntities checkoutEntities;
        private readonly ICartIdRetriever cartIdRetriever;

        public CartRetriever(CheckoutEntities checkoutEntities,
            ICartIdRetriever cartIdRetriever)
        {
            this.checkoutEntities = checkoutEntities;
            this.cartIdRetriever = cartIdRetriever;
        }

        public ShoppingCart GetTheCurrentCart()
        {
            return new ShoppingCart(cartIdRetriever);
        }

    }
}