using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class BannerCreateVM
    {
       [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Tittle { get; set; }
        [Required]
        public bool IsLarge { get; set; }

    }
}
