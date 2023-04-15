using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
