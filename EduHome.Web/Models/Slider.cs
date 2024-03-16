namespace EduHome.Web.Models
{
    public sealed class Slider : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Image { get; set; }
    }
}
