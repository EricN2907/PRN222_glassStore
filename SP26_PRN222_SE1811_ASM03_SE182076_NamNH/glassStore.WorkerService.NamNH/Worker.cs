using glassStore.Service.NamNH.Interface;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace glassStore.WorkerService.NamNH
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOrdersNamNhService _order;

        public Worker(ILogger<Worker> logger , IOrdersNamNhService order)
        {
            _logger = logger;
            _order = order;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}


                // My busines logic here

                await this.WriteToFile();

                await Task.Delay(5000, stoppingToken);
            }
        }
        protected async Task WriteToFile()
        {
            var item = await _order.GetAllAsync();  
            var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            var content = JsonSerializer.Serialize(item, opt);
            var filePath = @"D:\DataLog_PRN222.txt";
            using(var file = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using(var writer = new StreamWriter(file))
                {
                    await writer.WriteLineAsync(content);
                    await writer.FlushAsync();
                }
            }
        }
    }
}
