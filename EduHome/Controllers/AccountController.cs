using EduHome.Helper;
using EduHome.Models;
using EduHome.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class AccountController : Controller
    {
        #region Constructor
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

        } 
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            if (user.IsDeactive)
            {
                ModelState.AddModelError("", "Your account is deactive");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemember, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is blocked");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is blocked");
                return View();
            }
            return RedirectToAction("Index", "Home");
        } 
        #endregion

        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterVM registerVM)
        {
            AppUser newUser = new AppUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
                Name = registerVM.Name,
                Surname = registerVM.Surname
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser, registerVM.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        //#region CreateRoles
        //public async Task CreateRoles()
        //{
        //    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
        //    }
        //} 
        //#endregion

    }
}
