using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM
            {
                Sliders = await _db.Sliders.Where(x => !x.isDeactive).ToListAsync(),
                Services = await _db.Services.Where(x => !x.isDeactive).ToListAsync()
            };
            return View(vm);
        }
    }
}
