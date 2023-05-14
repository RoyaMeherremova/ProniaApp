using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ProductDetailVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public  Product ProductDetail { get; set; }

        public List<Advertising> Advertisings { get; set; }

        public List<Product> RelatedProducts { get; set; }

        public List<ProductComment> ProductComments { get; set; }    

        public CommentVM CommentVM { get; set; }


    }
}
