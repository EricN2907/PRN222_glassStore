using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using glassStore.RazorWebApp.NamNH.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{

    public class CreateModel : PageModel
    {
        private readonly IOrdersNamNhService _service;
        private readonly IUserService _userService;
        private readonly Order_Detail_NamNHRepositories _serviceSub;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public CreateModel(IOrdersNamNhService service, IUserService userService, Order_Detail_NamNHRepositories repo, IHubContext<glassStore_Hub> hubContext)
        {
            _service = service;
            _userService = userService;
            _serviceSub = repo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGet()
        {
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");
            
            var orders = await _service.GetAllAsync();
            ViewData["order_id"] = new SelectList(orders, "OrderId", "OrderCode");
            ViewData["VoucherId"] = new SelectList(Enumerable.Empty<object>(), "VoucherId", "Code");
            return Page();
        }

        [BindProperty]
        public OrdersNamNh OrdersNamNh { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
             // Manual Validation to ensure "nhập đủ mới lưu"
            if (string.IsNullOrWhiteSpace(OrdersNamNh.OrderCode)) ModelState.AddModelError("OrdersNamNh.OrderCode", "Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.OrderType)) ModelState.AddModelError("OrdersNamNh.OrderType", "Vui lòng chọn loại đơn hàng.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.PaymentMethod)) ModelState.AddModelError("OrdersNamNh.PaymentMethod", "Vui lòng chọn phương thức thanh toán.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.Status)) ModelState.AddModelError("OrdersNamNh.Status", "Vui lòng chọn trạng thái.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.ReceiverName)) ModelState.AddModelError("OrdersNamNh.ReceiverName", "Tên người nhận không được để trống.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.ReceiverPhone)) ModelState.AddModelError("OrdersNamNh.ReceiverPhone", "Số điện thoại không được để trống.");
            if (string.IsNullOrWhiteSpace(OrdersNamNh.ReceiverAddress)) ModelState.AddModelError("OrdersNamNh.ReceiverAddress", "Địa chỉ giao hàng không được để trống.");
            if (OrdersNamNh.UserId == null || OrdersNamNh.UserId == 0) ModelState.AddModelError("OrdersNamNh.UserId", "Vui lòng chọn khách hàng.");
            if (OrdersNamNh.Subtotal == null || OrdersNamNh.Subtotal <= 0) ModelState.AddModelError("OrdersNamNh.Subtotal", "Tạm tính phải lớn hơn 0.");
            if (OrdersNamNh.GrandTotal == null || OrdersNamNh.GrandTotal <= 0) ModelState.AddModelError("OrdersNamNh.GrandTotal", "Tổng cộng phải lớn hơn 0.");

            if (!ModelState.IsValid)
            {
                var users = await _userService.GetAllAsync();
                ViewData["UserId"] = new SelectList(users, "UserId", "Email");
                
                var orders = await _service.GetAllAsync();
                ViewData["order_id"] = new SelectList(orders, "OrderId", "OrderCode");
                ViewData["VoucherId"] = new SelectList(Enumerable.Empty<object>(), "VoucherId", "Code");
                return Page();
            }

            //_context.OrdersNamNhs.Add(OrdersNamNh);
            //await _context.SaveChangesAsync();
            var result = await _service.CreateAsync(OrdersNamNh);
            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveCreate_OrdersNamNH", OrdersNamNh);
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Create Failed");
                return Page();
            }
        }
    }
}
