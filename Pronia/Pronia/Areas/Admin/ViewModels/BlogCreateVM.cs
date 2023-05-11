using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class BlogCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public string Tittle { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public List<IFormFile> Photos { get; set; }

        public int AuthorId { get; set; }

        //public ICollection<Comment> Comments { get; set; }




    }
}
