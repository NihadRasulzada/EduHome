using EduHome.Helper;
using EduHome.Models;
using EduHome.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        #region Constructor
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> dbUsers = await _userManager.Users.ToListAsync();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser dbUser in dbUsers)
            {
                UserVM userVM = new UserVM
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
                    Surname = dbUser.Surname,
                    Email = dbUser.Email,
                    Username = dbUser.UserName,
                    IsDeactive = dbUser.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(dbUser))[0],
                };
                usersVM.Add(userVM);
            }
            return View(usersVM);
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }

            if (!dbUser.IsDeactive)
            {
                dbUser.IsDeactive = true;
            }
            else
            {
                dbUser.IsDeactive = false;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM createVM, string role)
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            AppUser newUser = new AppUser
            {
                UserName = createVM.Username,
                Email = createVM.Email,
                Name = createVM.Name,
                Surname = createVM.Surname
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, createVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, role);
            return RedirectToAction("Index");
        }
        #endregion

        public async Task<IActionResult> Update(string id)
        {
            #region Get
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Email = dbUser.Email,
                Surname = dbUser.Surname,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            #endregion

            return View(dbUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string role)
        {
            #region Get
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Email = dbUser.Email,
                Surname = dbUser.Surname,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString()
            };
            #endregion

            dbUser.Name = updateVM.Name;
            dbUser.UserName = updateVM.Username;
            dbUser.Surname = updateVM.Surname;
            dbUser.Email = updateVM.Email;

            if (dbUpdateVM.Role != role)
            {
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(dbUser, dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(dbUser, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
