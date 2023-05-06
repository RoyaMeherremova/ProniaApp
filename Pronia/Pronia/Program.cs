using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>  
{
    opt.Password.RequiredLength = 8;  
    opt.Password.RequireDigit = true;  
    opt.Password.RequireLowercase = true; 
    opt.Password.RequireUppercase = true; 
    opt.Password.RequireNonAlphanumeric = true; 

    opt.User.RequireUniqueEmail = true;  
    opt.SignIn.RequireConfirmedEmail = true;

    opt.Lockout.MaxFailedAccessAttempts = 3; 
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); 
    opt.Lockout.AllowedForNewUsers = true; 


});

builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<ISocialService, SocialService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAdvertisingService, AdvertisingService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
