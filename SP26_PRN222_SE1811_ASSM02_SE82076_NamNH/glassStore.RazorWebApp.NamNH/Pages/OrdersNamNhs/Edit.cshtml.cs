using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using glassStore.RazorWebApp.NamNH.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class EditModel : PageModel
    {
        //private readonly glassStore.Entites.NamNH.Models.glass_StoreContext _context;

        private readonly IOrdersNamNhService _service;
        private readonly IUserService _userService;
        private readonly OrderDetailNamNhService _detail;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public EditModel(IOrdersNamNhService service, OrderDetailNamNhService detail, IUserService userService, IHubContext<glassStore_Hub> hubContext)
        {
            _service = service;
            _detail = detail;
            _userService = userService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public OrdersNamNh OrdersNamNh { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ordersnamnh =  await _context.OrdersNamNhs.FirstOrDefaultAsync(m => m.OrderId == id);
                
            var ordersnamnh = await _service.GetByIdAsync(id.Value);
            if (ordersnamnh == null)
            {
                return NotFound();
            }
            OrdersNamNh = ordersnamnh;
            
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");

            var orderDetails = await _detail.GetAllAsync();
            ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode");
            ViewData["VoucherId"] = new SelectList(Enumerable.Empty<object>(), "VoucherId", "Code");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Manual Validation to ensure "nhập đủ mới lưu"
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
                ViewData["UserId"] = new SelectList(users, "UserId", "Email", OrdersNamNh.UserId);
                
                var orderDetails = await _detail.GetAllAsync();
                ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode");
                ViewData["VoucherId"] = new SelectList(Enumerable.Empty<object>(), "VoucherId", "Code");
                return Page();
            }
            try
            {
                var result = await _service.UpdateAsync(OrdersNamNh);
                if(result > 0)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveUpdate_OrdersNamNH", OrdersNamNh);
                    return RedirectToPage("./Index");
                }

            }
           
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            //return RedirectToPage("./Index");
            return Page();
        }

        //private bool OrdersNamNhExists(int id)
        //{
        //    return _context.OrdersNamNhs.Any(e => e.OrderId == id);
        //}
    }
}
