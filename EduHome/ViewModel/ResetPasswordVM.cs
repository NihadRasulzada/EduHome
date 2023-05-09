using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModel
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }
        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu xana bosh ola bilmez")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
    }
}
