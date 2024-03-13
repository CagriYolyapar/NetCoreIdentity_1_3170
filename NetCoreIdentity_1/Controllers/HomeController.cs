using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity_1.Models;
using NetCoreIdentity_1.Models.ContextClasses;
using NetCoreIdentity_1.Models.Entities;
using NetCoreIdentity_1.Models.ViewModels.AppUsers.PureVms;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

                //Mapping işlemi (UserRegisterRequestModel tipindeki model'deki bilgileri AppUser class'ından instance alarak oraya aktarıyoruz)
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

                    return RedirectToAction("Register");

                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }
            return View(model);
        }

        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new() 
            {
                ReturnUrl = returnUrl
            };
            return View(usModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName); //await ile bir Task'in direkt sonucunu beklediginiz icin onu elde edersiniz...

                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);
                
                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel"); //Eger gitmek istediginiz sayfa bir baska Area'da ise routeData parametresine Anonymus type olarak argüman vererek gönderirsiniz return RedirectToAction("AdminPanel",new {Area = "Admin"})
                    }
                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("MemberPanel");
                    }

                    return RedirectToAction("Panel");

                    // localhost:1244//Admin/Home/Index/2

                }
                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);

                    ModelState.AddModelError("", $"Hesabınız {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika süreyle askıya alınmıstır");
                }
                else
                {
                    string message = "";
                    if(appUser != null)
                    {
                        int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts; //middleware'deki maksimum hata sayınız...

                        message = $"Eger {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanlıs giriş yaparsanız hesabınız gecici olarak askıya alınacaktır";
                    }
                    else
                    {
                        message = "Kullanıcı bulunamadı";
                    }

                    ModelState.AddModelError("", message);
                    
                }

            }
            return View(model);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles ="Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }

    }
}
