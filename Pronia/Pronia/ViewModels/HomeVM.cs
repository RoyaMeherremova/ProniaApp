using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Advertising> Advertising { get; set; }

        public Dictionary<string, string> HeaderBackgrounds { get; set; }

        public List<Product> FeaturedProduct { get; set; }

        public List<Product> BestsellerProduct { get; set; }
        public List<Product> LatestProduct { get; set; }
        public List<Bannner> Banners { get; set; }

        public List<Product> NewProducts { get; set; }


    }
}
