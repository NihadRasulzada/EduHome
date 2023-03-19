using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModel;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            List<Slider> slider = _db.Sliders.ToList();
            List<Service> services = _db.Services.ToList();
            HomeVM vm = new HomeVM();
            vm.Sliders = slider;
            vm.Services = services;
            return View(vm);
        }
    }
}
