using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using glassStore.WorkerService.NamNH;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "NamNH.WS";
});
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IOrdersNamNhService, OrdersNamNhService>();
builder.Services.AddScoped<Order_Detail_NamNHRepositories>();
builder.Services.AddScoped<OrderDetailNamNhService>();

var host = builder.Build();
host.Run();
