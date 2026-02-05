using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace glassStore.RazorWebApp.NamNH.Hubs
{
    public class glassStore_Hub : Hub
    {
        private readonly IOrdersNamNhService _service;
        private readonly OrderDetailNamNhService _details;

        public glassStore_Hub(IOrdersNamNhService service, OrderDetailNamNhService details)
        {
            _service = service;
            _details = details;
        }

        public async Task hubDelete_OrdersNamNH(int orderId)
        {
            try
            {
                bool isDeleted = await _service.DeleteAsync(orderId);

                if (isDeleted)
                {
                    await Clients.Caller.SendAsync("DeleteSuccess");
                    await Clients.Others.SendAsync("ReceiveDelete_OrdersNamNH", orderId);
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Không tìm thấy đơn hàng để xóa.");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Lỗi DB: " + ex.Message);
            }
        }
    }
}
