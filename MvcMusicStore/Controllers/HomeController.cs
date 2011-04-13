using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Shared.Helpers;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITopSellingProductsCalculator topSellingProductsCalculator;
        //
        // GET: /Home/

        MusicStoreEntities storeDB = new MusicStoreEntities();

        public HomeController(ITopSellingProductsCalculator topSellingProductsCalculator)
        {
            this.topSellingProductsCalculator = topSellingProductsCalculator;
        }

        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);

            return View(albums);
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            var items = topSellingProductsCalculator.GetTheKeysOfTheTopSellingProducts();
            return (from album in storeDB.Albums.ToList()
                   join item in items on album.AlbumId equals item
                   select album).ToList();

        }
    }
}