using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using glassStore.BlazorWebApp.NamNH.Security;
using Microsoft.EntityFrameworkCore;
using glassStore.Service.NamNH;
using glassStore.Repositories.NamNH;
using glassStore.BlazorWebApp.NamNH.Components;
using glassStore.BlazorWebApp.NamNH.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<glassStore.Entites.NamNH.Models.glass_StoreContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<glassStore.Entites.NamNH.Models.glass_StoreContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrdersNamNhService, OrdersNamNhService>();
builder.Services.AddScoped<OrdersNamNhRepositories>();
builder.Services.AddScoped<Order_Detail_NamNHRepositories>();
builder.Services.AddScoped<OrderDetailNamNhService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserRepositories>();

builder.Services.AddSignalR();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<glassStore.BlazorWebApp.NamNH.Hubs.glassStore_Hub>("/glassStore_Hub");

app.Run();
