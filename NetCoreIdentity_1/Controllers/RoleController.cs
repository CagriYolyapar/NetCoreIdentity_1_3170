using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity_1.Models.Admins.AppRoles.RequestModels;
using NetCoreIdentity_1.Models.Entities;

namespace NetCoreIdentity_1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequestModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result =  await _roleManager.CreateAsync(new()
                {
                    Name = model.RoleName
                });

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}


/*
 * Manager
 * 
 * KDV
 * Gumruk
 * 
 * 
 * 
 * 
 
 EFBaseRepository                             AccessRepository
                                            _db.Set
_db.Set<T>


 
 
 
 
 
 */