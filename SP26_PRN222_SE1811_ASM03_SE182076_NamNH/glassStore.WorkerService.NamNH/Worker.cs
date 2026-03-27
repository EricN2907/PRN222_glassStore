using glassStore.Service.NamNH.Interface;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace glassStore.WorkerService.NamNH
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NamNH_WorkerService started at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var order = scope.ServiceProvider.GetRequiredService<IOrdersNamNhService>();
                    await this.WriteToFile(order);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
        protected async Task WriteToFile(IOrdersNamNhService _order)
        {
            try
            {
                var item = await _order.GetAllAsync();
                var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true };
                var content = JsonSerializer.Serialize(item, opt);
                var filePath = @"D:\DataLog_PRN222.txt";

                // Use FileMode.Create to overwrite the file each time, preventing duplication.
                // Added FileShare.ReadWrite to be more permissive.
                using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        await writer.WriteLineAsync(content);
                        await writer.FlushAsync();
                    }
                }
            }
            catch (IOException ex)
            {
                _logger.LogWarning("Could not write to log file - it might be open in another program: {message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while writing to log file.");
            }
        }
    }
}
