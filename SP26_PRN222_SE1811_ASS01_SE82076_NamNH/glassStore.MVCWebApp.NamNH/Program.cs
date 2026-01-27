using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add db
builder.Services.AddDbContext<glass_StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add depenency injection
builder.Services.AddScoped<IOrdersNamNhService, OrdersNamNhService>();
builder.Services.AddScoped<OrderDetailNamNhService>();
builder.Services.AddScoped<SystemUserAccountService>();

//Add Authentication
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
