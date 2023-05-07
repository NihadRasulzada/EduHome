using EduHome.DAL;
using EduHome.Helper;
using EduHome.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _db.Sliders.ToListAsync();
            return View(slider);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image is empty");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Is not image");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Img = await slider.Photo.SaveFileAsync(folder);

            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbslider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbslider == null)
            {
                return BadRequest();
            }

            if (!dbslider.isDeactive)
            {
                dbslider.isDeactive = true;
            }
            else
            {
                dbslider.isDeactive = false;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Slider _dbslider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (_dbslider == null)
            {
                return BadRequest();
            }
            return View(_dbslider);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            return View(dbSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Is not image");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.Img = await slider.Photo.SaveFileAsync(folder);
                string path = Path.Combine(_env.WebRootPath, folder, dbSlider.Img);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                dbSlider.Img = slider.Img;
            }
            dbSlider.Name = slider.Name;
            dbSlider.Description = slider.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
        #endregion

    }
}
