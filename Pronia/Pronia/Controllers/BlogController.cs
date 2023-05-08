﻿using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            BlogVM model = new()
            {
                HeaderBackgrounds = headerBackgrounds,
            };
           

            return View(model);
        }
    }
}
