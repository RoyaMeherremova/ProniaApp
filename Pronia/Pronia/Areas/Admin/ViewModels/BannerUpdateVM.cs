using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class BannerUpdateVM
    {
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Tittle { get; set; }

        public bool IsLarge { get; set; }
    }
}
