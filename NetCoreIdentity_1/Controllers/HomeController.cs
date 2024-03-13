﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity_1.Models;
using NetCoreIdentity_1.Models.ContextClasses;
using NetCoreIdentity_1.Models.Entities;
using NetCoreIdentity_1.Models.ViewModels.AppUsers.PureVms;
using System.Diagnostics;

namespace NetCoreIdentity_1.Controllers
{
    [AutoValidateAntiforgeryToken] //Get ile gelen sayfada verilen özel bir token sayesinde Post'un bu tokensiz yapılamamasını saglar...PostMan gibi third part software'lerinden gözlemlediginizde direkt Post tarafına ulasamadıgınızı göreceksiniz...
    public class HomeController : Controller

    //Identity kütüphanesi crud ve servis işlemleri icin bir takım class'lara sahiptir... Bu Manager Class'ları sizin ilgili Identity yapılarınızın Crud işlemlerine ve baska business logic işlemlerine girmesini saglarlar...
    {
        private readonly ILogger<HomeController> _logger;

        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Todo : AntiforgerToken gözlemlenecek

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    #region AdminEklemekIcinTekKullanimlikKodlar
                    //AppRole role = await _roleManager.FindByNameAsync("Admin"); //Admin ismindeki rolu bulabilirse Role nesnesini appRole'e atacak bulamazsa appRole null olacak
                    //if (role == null) await _roleManager.CreateAsync(new() { Name = "Admin" });//Admin isminde bir rol yarattık

                    //await _userManager.AddToRoleAsync(appUser, "Admin"); //appUser degişkeninin tuttugu kullanıcı nesnesini Admin isimli Role'e ekledik...

                    #endregion

                    #region MemberEklemekIcinKodlar
                    AppRole role = await _roleManager.FindByNameAsync("Member");
                    if (role == null) await _roleManager.CreateAsync(new() { Name = "Member" });

                    await _userManager.AddToRoleAsync(appUser, "Member"); //Register olan kullanıcı artık bu kod sayesinde direkt Member rolüne sahip olacaktır... 
                    #endregion

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
