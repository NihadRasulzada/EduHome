using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewComponents
{
    public class TeachersViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public TeachersViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take)
        {
            List<Teacher> teachers;
            if(take > 0) {
                teachers = await _db.Teachers.Take(take).ToListAsync();
                return View(teachers);
            }
            teachers = await _db.Teachers.ToListAsync();
            return View(teachers);
        }
    }
}
