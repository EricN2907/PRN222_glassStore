using glassStore.Entites.NamNH.Models;
using glassStore.RazorWebApp.NamNH.Hubs;
using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

//add db
builder.Services.AddDbContext<glass_StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//add DI 
builder.Services.AddScoped<IOrdersNamNhService, OrdersNamNhService>();
builder.Services.AddScoped<Order_Detail_NamNHRepositories>();
builder.Services.AddScoped<OrderDetailNamNhService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Account/Forbidden";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

var app = builder.Build();
    
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapHub<glassStore_Hub>("/glassStore_Hub");

app.Run();
