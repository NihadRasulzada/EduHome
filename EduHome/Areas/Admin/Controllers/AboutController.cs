using EduHome.DAL;
using EduHome.Helper;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            About about = await _db.About.FirstOrDefaultAsync();
            return View(about);
        }

        #region Detail
        public async Task<IActionResult> Detail()
        {
            About about = await _db.About.FirstOrDefaultAsync();
            return View(about);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update()
        {
            About about = await _db.About.FirstOrDefaultAsync();
            return View(about);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(About about)
        {
            About dbabout = await _db.About.FirstOrDefaultAsync();
            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Is not image");
                    return View();
                }
                if (!about.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Is not Older 1Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "about");
                about.Img = await about.Photo.SaveFileAsync(folder);
                string path = Path.Combine(_env.WebRootPath, folder, dbabout.Img);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                dbabout.Img = about.Img;
            }
            dbabout.Title = about.Title;
            dbabout.Description = about.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
        #endregion

    }
}
