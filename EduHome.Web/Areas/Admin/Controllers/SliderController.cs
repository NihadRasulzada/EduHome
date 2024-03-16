using EduHome.Web.DateAccessLayer;
using EduHome.Web.Extensions;
using EduHome.Web.Helpers;
using EduHome.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _appDbContext.Sliders.ToListAsync();
            return View(sliders);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            if (slider.Photo != null)
            {
                if (!slider.Photo.CheckFileContenttype("image"))
                {
                    ModelState.AddModelError("Photo", $"{slider.Photo.FileName} adli fayl novu duzgun deyil");
                    return View(slider);
                }
                if (slider.Photo.CheckFileLength(5000))
                {
                    ModelState.AddModelError("Photo", $"{slider.Photo.FileName} adli fayl hecmi coxdur");
                    return View(slider);
                }
                slider.Image = await slider.Photo.CreateFileAsync(_env, "img", "slider");
            }

            slider.CreatedTime = DateTime.UtcNow.AddHours(4);
            slider.CreatedBy = "System";

            await _appDbContext.Sliders.AddAsync(slider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Slider");
        }
        #endregion

        public async Task<IActionResult> Update(Nullable<int> id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider? dbSlider = await _appDbContext.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            }
            return View(dbSlider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Nullable<int> id, Slider slider)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider? dbSlider = await _appDbContext.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            if (slider.Photo != null)
            {
                if (!slider.Photo.CheckFileContenttype("image"))
                {
                    ModelState.AddModelError("Photo", $"{slider.Photo.FileName} adli fayl novu duzgun deyil");
                    return View(slider);
                }
                if (slider.Photo.CheckFileLength(5000))
                {
                    ModelState.AddModelError("Photo", $"{slider.Photo.FileName} adli fayl hecmi coxdur");
                    return View(slider);
                }
                slider.Image = await slider.Photo.CreateFileAsync(_env, "img", "slider");
                FileHelper.DeleteFile(dbSlider.Image, _env, "img", "slider");
            }
            else
            {
                slider.Image = dbSlider.Image;
            }

            _appDbContext.Entry(dbSlider).CurrentValues.SetValues(slider);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Slider");
        }
    }
}
