using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class SocialUpdateVM
    {
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public string Link { get; set; }
    }
}
