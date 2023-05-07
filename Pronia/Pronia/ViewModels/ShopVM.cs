﻿using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ShopVM
    {
        public List<Category> Categories { get; set; }

        public List<Product> NewProducts { get; set;}

        public List<Color> Colors { get; set; }

        public Dictionary<string, string> HeaderBackgrounds { get; set; }

       public  List<Product> Products { get; set; }

    }
}
