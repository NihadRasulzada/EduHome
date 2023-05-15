using EduHome.DAL;
using EduHome.Helper;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        #region CTOR
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CoursesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _db.Courses.ToListAsync();
            return View(courses);
        } 
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbcourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbcourse == null)
            {
                return BadRequest();
            }

            if (!dbcourse.IsDeactive)
            {
                dbcourse.IsDeactive = true;
            }
            else
            {
                dbcourse.IsDeactive = false;
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
            Course dbcourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbcourse == null)
            {
                return BadRequest();
            }
            return View(dbcourse);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (course.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image is empty");
                return View();
            }
            if (!course.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Is not image");
                return View();
            }
            if (!course.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Is not Older 1Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img", "course");
            course.Img = await course.Photo.SaveFileAsync(folder);

            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbcourse = _db.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (dbcourse == null)
            {
                return BadRequest();
            }
            return View(dbcourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Course course)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbcourse = _db.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (dbcourse == null)
            {
                return BadRequest();
            }
            if (course.Photo != null)
            {
                if (!course.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Is not image");
                    return View();
                }
                if (!course.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Is not older 1Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "course");
                course.Img = await course.Photo.SaveFileAsync(folder);
                string path = Path.Combine(_env.WebRootPath, folder, dbcourse.Img);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                dbcourse.Img = course.Img;
            }
            dbcourse.Name = course.Name;
            dbcourse.Description = course.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
