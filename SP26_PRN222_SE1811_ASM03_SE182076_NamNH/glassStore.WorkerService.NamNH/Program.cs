using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using glassStore.WorkerService.NamNH;
var builder = Host.CreateApplicationBuilder(args);
// 1. Cấu hình tên Service
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Vu635";
});
builder.Logging.AddEventLog(settings =>
{
    settings.SourceName = "Vu635"; 
});

// Force EventLog to accept Information level (default is Warning+)
builder.Logging.AddFilter<Microsoft.Extensions.Logging.EventLog.EventLogLoggerProvider>(null, LogLevel.Information);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IOrdersNamNhService, OrdersNamNhService>();
builder.Services.AddScoped<Order_Detail_NamNHRepositories>();
builder.Services.AddScoped<OrderDetailNamNhService>();
var host = builder.Build();
host.Run();

//dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
/*
 sc.exe create Vu635 binPath= "D:\FPT\PRN222\GlassesShoppingCart\SP26_PRN222_SE1811_ASM03_SE182076_NamNH\glassStore.WorkerService.NamNH\bin\Release\net8.0\win-x64\publish\glassStore.WorkerService.NamNH.exe"
powershell -Command "New-EventLog -LogName Application -Source 'VuNLS2'"

sc.exe start NamNH_WorkerService1

 */