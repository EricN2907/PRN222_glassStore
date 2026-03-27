using Microsoft.AspNetCore.SignalR;
using glassStore.Entites.NamNH.Models;

namespace glassStore.BlazorWebApp.NamNH.Hubs
{
    public class glassStore_Hub : Hub
    {
        // Gửi thông báo khi tạo mới đơn hàng
        public async Task SendCreate_OrdersNamNH(OrdersNamNh order)
        {
            await Clients.All.SendAsync("ReceiveCreate_OrdersNamNH", order);
        }

        // Gửi thông báo khi cập nhật đơn hàng
        public async Task SendUpdate_OrdersNamNH(OrdersNamNh order)
        {
            // Bảo đảm order.OrderId có giá trị
            await Clients.All.SendAsync("ReceiveUpdate_OrdersNamNH", order);
        }

        // Gửi thông báo khi đơn hàng bị xóa
        public async Task SendDelete_OrdersNamNH(int orderId)
        {
            await Clients.All.SendAsync("ReceiveDelete_OrdersNamNH", orderId);
        }
    }
}
