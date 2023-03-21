using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHome.ViewComponents
{
    public class TestimonialsViewComponent: ViewComponent
    {
        private readonly AppDbContext _db;
        public TestimonialsViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Testimonial> testimonials = await _db.Testimonials.ToListAsync();
            return View(testimonials);
        }
    }
}
