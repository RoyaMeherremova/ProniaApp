﻿using Microsoft.AspNetCore.Mvc;
using Pronia.Data;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            AboutVM model = new()
            {
               HeaderBackgrounds= headerBackgrounds,
            };
            return View(model);
        }
    }
}
