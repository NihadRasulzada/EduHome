using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModel
{
    public class UpdateVM
    {
        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
