using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Web.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeactive { get; set; }
    }
}
