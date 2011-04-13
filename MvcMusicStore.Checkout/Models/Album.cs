using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MvcMusicStore.Checkout.Models
{
    [Bind(Exclude = "AlbumId")]
    public class Album
    {
        public int      AlbumId    { get; set; }
        public string   Title      { get; set; }
        public decimal Price       { get; set; }
        public string AlbumArtUrl { get; set; }
    }
}