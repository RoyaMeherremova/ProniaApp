using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class BlogUpdateVM
    {
        public int Id { get; set; }
        public List<BlogImage> Images { get; set; }

        [Required(ErrorMessage = "Don't be empty")]
        public string Tittle { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }

        public int AuthorId { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
