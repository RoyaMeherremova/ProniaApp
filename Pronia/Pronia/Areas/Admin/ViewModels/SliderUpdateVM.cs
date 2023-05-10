using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class SliderUpdateVM
    {
         public string Image { get; set; }
        public IFormFile Photo { get; set; }
        [Required]
        public string Tittle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Offer { get; set; }

    }
}
