using EduHome.Web.DateAccessLayer;
using EduHome.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _appDbContext.Sliders.Where(x => x.IsDeactive == false).ToListAsync();
            return View(sliders);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
