using Pronia.Helpers;
using Pronia.Models;

namespace Pronia.ViewModels
{
    public class BlogVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }

        public List<Blog> Blogs { get; set; }

        public Paginate<Blog> PaginatedDatas { get; set; }
        public List<Category> Categories { get; set; }

        public List<Tag> Tags { get; set; }
        public List<Product> NewProducts { get; set; }
    }
}
