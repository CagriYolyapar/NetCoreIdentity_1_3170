﻿using Microsoft.EntityFrameworkCore;
using NetCoreIdentity_1.Models.ContextClasses;
using NetCoreIdentity_1.Models.Entities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequiredLength = 3;
    x.Password.RequireDigit = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Lockout.MaxFailedAccessAttempts = 5;
    x.User.RequireUniqueEmail = true;
    //x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
   
}).AddEntityFrameworkStores<MyContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    x.Cookie.Name = "CyberSelf";
    x.ExpireTimeSpan = TimeSpan.FromDays(1);
    x.Cookie.SameSite = SameSiteMode.Strict;
    x.LoginPath = new PathString("/Home/SignIn");
    x.AccessDeniedPath = new PathString("/Home/AccessDenied");
});


builder.Services.AddDbContextPool<MyContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());



WebApplication app = builder.Build();





//Since when ambiouty is a good practice in software development

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Burada sakın Authentication'i unutmayın...CÜnkü Authorization'dan(Yetkiden önce) kimlik tanınması gerekir (Authentication)

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");

app.Run();
