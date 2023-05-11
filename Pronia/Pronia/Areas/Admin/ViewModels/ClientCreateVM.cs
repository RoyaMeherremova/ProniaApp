﻿using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class ClientCreateVM
    {
        [Required(ErrorMessage ="Don't be empty")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
   
      

    }
}
