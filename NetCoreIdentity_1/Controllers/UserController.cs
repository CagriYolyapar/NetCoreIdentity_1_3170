using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreIdentity_1.Models.Entities;

namespace NetCoreIdentity_1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            //List<AppUser> allUsers =  _userManager.Users.ToList();

            
            List<AppUser> nonAdminUsers = _userManager.Users.Where(x => !x.UserRoles.Any(x => x.Role.Name =="Admin")).ToList();

            return View(nonAdminUsers);
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            IList<string> userRoles = await  _userManager.GetRolesAsync(appUser); // Elimize gecen kullanıcının rollerini verir

            List<AppRole> allRoles = _roleManager.Roles.ToList(); //bütün roller
        }
    }
}
