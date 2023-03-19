using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
