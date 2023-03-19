using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHome.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public SliderViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public  async Task<IViewComponentResult> InvokeAsync()
        {
            List<Slider> sliderList = await _db.Sliders.ToListAsync();
            return View(sliderList);
        }
    }
}
