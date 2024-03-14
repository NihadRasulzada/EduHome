using EduHome.Web.DateAccessLayer;
using EduHome.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public SliderController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _appDbContext.Sliders.ToListAsync();
            return View(sliders);
        }
    }
}
