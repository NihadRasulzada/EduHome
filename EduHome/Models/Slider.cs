using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool isDeactive { get; set; }
    }
}
