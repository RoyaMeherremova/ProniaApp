using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }    

    }
}
