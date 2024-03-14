using EduHome.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Web.DateAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slider>().Ignore(x => x.Photo);
            modelBuilder.Entity<Slider>().Property(x => x.IsDeactive).HasDefaultValue(false);
        }
    }
}
