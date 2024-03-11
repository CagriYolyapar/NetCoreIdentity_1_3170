using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreIdentity_1.Models.Entities;

namespace NetCoreIdentity_1.Models.ContextClasses
{
    //Identity kullanacak iseniz IdentityDbContext class'indan miras almalısınız ve  eger 3'ten fazla Identity class'i customize ettiyseniz IdentityDbContext'in generic yapısının tüm tiplerini acıkca vermek zorundasınız...
    public class MyContext:IdentityDbContext<AppUser,AppRole,int,IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {


    }
}
