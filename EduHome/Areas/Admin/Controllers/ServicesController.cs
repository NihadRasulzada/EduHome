using EduHome.DAL;
using EduHome.Helper;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;
        public ServicesController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _db.Services.ToListAsync();
            return View(services);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            bool isExist = await _db.Services.AnyAsync(x => x.Name == service.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Service is allready exist");
                return View();
            }

            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            Service _dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            return View(_dbService);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Service service)
        {
            Service _dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            if (_dbService == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.Services.AnyAsync(x => x.Name == service.Name && x.Id != service.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Service is allready exist");
                return View();
            }
            _dbService.Name = service.Name;
            _dbService.Description = service.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            Service _dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            if (_dbService == null)
            {
                return BadRequest();
            }
            return View(_dbService);
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }

            if (!dbService.isDeactive)
            {
                dbService.isDeactive = true;
            }
            else
            {
                dbService.isDeactive = false;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
        #endregion
    }
}
