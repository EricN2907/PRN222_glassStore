using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
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

        public async Task hubUpdate_OrdersNamNH(Entites.NamNH.Models.OrdersNamNh order)
        {
            try
            {
                int result = await _service.UpdateAsync(order);

                if (result > 0)
                {
                    await Clients.Caller.SendAsync("UpdateSuccess");
                    await Clients.Others.SendAsync("ReceiveUpdate_OrdersNamNH", order);
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Không thể cập nhật đơn hàng.");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Lỗi DB: " + ex.Message);
            }
        }
        public async Task hubCreate_OrdersNamNH(Entites.NamNH.Models.OrdersNamNh order)
        {
            try
            {
                int result = await _service.CreateAsync(order);

                if (result > 0)
                {
                    await Clients.Caller.SendAsync("CreateSuccess");
                    await Clients.Others.SendAsync("ReceiveCreate_OrdersNamNH", order);
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Không thể tạo đơn hàng.");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveErrorNamNH", "Lỗi DB: " + ex.Message);
            }
        }
       
    }
}
