using System;

namespace EduHome.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string By { get; set;}
        public DateTime Date { get; set; }
        public string NotficationCount { get; set; }

    }
}
