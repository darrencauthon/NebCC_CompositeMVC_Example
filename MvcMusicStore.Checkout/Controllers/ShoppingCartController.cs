using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Checkout.Helpers;
using MvcMusicStore.Checkout.Models;
using MvcMusicStore.Checkout.ViewModels;

namespace MvcMusicStore.Checkout.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ICartIdRetriever cartIdRetriever;
        CheckoutEntities storeDB = new CheckoutEntities();

        public ShoppingCartController(ICartIdRetriever cartIdRetriever)
        {
            this.cartIdRetriever = cartIdRetriever;
        }

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = new ShoppingCart(cartIdRetriever);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View((object) viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedAlbum = storeDB.Albums
                .Single(album => album.AlbumId == id);

            // Add it to the shopping cart
            //var cart = ShoppingCart.GetCart(this.HttpContext);
            var cart = new ShoppingCart(cartIdRetriever);

            cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = new ShoppingCart(cartIdRetriever);

            // Get the name of the album to display confirmation
            string albumName = storeDB.Carts
                .Single(item => item.RecordId == id).Album.Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = new ShoppingCart(cartIdRetriever);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}
