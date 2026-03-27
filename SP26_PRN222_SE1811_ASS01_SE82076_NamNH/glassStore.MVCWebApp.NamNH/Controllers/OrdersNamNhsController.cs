using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH.Interface;
using glassStore.Service.NamNH;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using glassStore.MVCWebApp.NamNH.Hubs;

namespace glassStore.MVCWebApp.NamNH.Controllers
{
    [Authorize]
    public class OrdersNamNhsController : Controller
    {
        //private readonly glass_StoreContext _context;
        private readonly IOrdersNamNhService _orders;
        private readonly OrderDetailNamNhService _orderDetails;
        private readonly IUserService _userService;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public OrdersNamNhsController(IOrdersNamNhService ordersNamNhService, OrderDetailNamNhService orderDetailNamNh, IUserService userService, IHubContext<glassStore_Hub> hubContext) 
        {
            _orders = ordersNamNhService ?? new OrdersNamNhService();
            _orderDetails = orderDetailNamNh ?? new OrderDetailNamNhService();
            _userService = userService ?? new UserService();
            _hubContext = hubContext;
        }


        // GET: OrdersNamNhs
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Index(string order_code, string phone_number, string receiver_name, int pageNumber = 1)
        {
            int pageSize = 10;
            var items = await _orders.SearchAsync(order_code, phone_number, receiver_name, pageNumber, pageSize);
            var totalItems = await _orders.GetSearchCountAsync(order_code, phone_number, receiver_name);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.OrderCode = order_code;
            ViewBag.PhoneNumber = phone_number;
            ViewBag.ReceiverName = receiver_name;

            return View(items);
        }
       // [Authorize(Roles = "1, 2")]
        // GET: OrdersNamNhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            if (id == null) return NotFound();
            var order = await _orders.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }
      //  [Authorize(Roles = "1, 2")]
        // GET: OrdersNamNhs/Create
        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");

            var item = await _orders.GetAllAsync();
            ViewData["order_id"] = new SelectList(item, "OrderId", "OrderCode");
            return View();
        }

        // POST: OrdersNamNhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Create(OrdersNamNh ordersNamNh)
        {
            // Manual Validation to ensure "nhập đủ mới lưu"
            if (string.IsNullOrWhiteSpace(ordersNamNh.OrderCode)) ModelState.AddModelError("OrderCode", "Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.OrderType)) ModelState.AddModelError("OrderType", "Vui lòng chọn loại đơn hàng.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.PaymentMethod)) ModelState.AddModelError("PaymentMethod", "Vui lòng chọn phương thức thanh toán.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.Status)) ModelState.AddModelError("Status", "Vui lòng chọn trạng thái.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverName)) ModelState.AddModelError("ReceiverName", "Tên người nhận không được để trống.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverPhone)) ModelState.AddModelError("ReceiverPhone", "Số điện thoại không được để trống.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverAddress)) ModelState.AddModelError("ReceiverAddress", "Địa chỉ giao hàng không được để trống.");
            if (ordersNamNh.UserId == null || ordersNamNh.UserId == 0) ModelState.AddModelError("UserId", "Vui lòng chọn khách hàng.");
            if (ordersNamNh.Subtotal == null || ordersNamNh.Subtotal <= 0) ModelState.AddModelError("Subtotal", "Tạm tính phải lớn hơn 0.");
            if (ordersNamNh.GrandTotal == null || ordersNamNh.GrandTotal <= 0) ModelState.AddModelError("GrandTotal", "Tổng cộng phải lớn hơn 0.");

            if (ModelState.IsValid)
            {
                // Check duplicate OrderCode
                var existing = await _orders.SearchAsync(ordersNamNh.OrderCode, null, null);
                if (existing != null && existing.Any(o => o.OrderCode == ordersNamNh.OrderCode))
                {
                    ModelState.AddModelError("OrderCode", "Mã đơn hàng này đã tồn tại.");
                }
                else
                {
                    var result = await _orders.CreateAsync(ordersNamNh);
                    if (result > 0)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveCreate_OrdersNamNH", ordersNamNh);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email", ordersNamNh.UserId);

            var orderDetails = await _orderDetails.GetAllAsync();
            ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode");

            return View(ordersNamNh);
        }

      //  [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id) { 
        
            if(id == null)
            {
                return NotFound();
            }
            var item = await _orders.GetByIdAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email", item.UserId);

            return View(item);
        }


        //POST: OrdersNamNhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "1")] 
        public async Task<IActionResult> Edit(int id, OrdersNamNh ordersNamNh)
        {
            // Manual Validation to ensure "nhập đủ mới lưu"
            if (string.IsNullOrWhiteSpace(ordersNamNh.OrderType)) ModelState.AddModelError("OrderType", "Vui lòng chọn loại đơn hàng.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.PaymentMethod)) ModelState.AddModelError("PaymentMethod", "Vui lòng chọn phương thức thanh toán.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.Status)) ModelState.AddModelError("Status", "Vui lòng chọn trạng thái.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverName)) ModelState.AddModelError("ReceiverName", "Tên người nhận không được để trống.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverPhone)) ModelState.AddModelError("ReceiverPhone", "Số điện thoại không được để trống.");
            if (string.IsNullOrWhiteSpace(ordersNamNh.ReceiverAddress)) ModelState.AddModelError("ReceiverAddress", "Địa chỉ giao hàng không được để trống.");
            if (ordersNamNh.UserId == null || ordersNamNh.UserId == 0) ModelState.AddModelError("UserId", "Vui lòng chọn khách hàng.");
            if (ordersNamNh.Subtotal == null || ordersNamNh.Subtotal <= 0) ModelState.AddModelError("Subtotal", "Tạm tính phải lớn hơn 0.");
            if (ordersNamNh.GrandTotal == null || ordersNamNh.GrandTotal <= 0) ModelState.AddModelError("GrandTotal", "Tổng cộng phải lớn hơn 0.");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _orders.UpdateAsync(ordersNamNh);
                    if(result > 0)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveUpdate_OrdersNamNH", ordersNamNh);
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                catch (Exception ex) 
                {
                   var message = ex.InnerException != null ? ex.Message + " | Inner: " + ex.InnerException.Message : ex.Message;
                   throw new Exception(message);
                }
            }
            var users = await _userService.GetAllAsync();
            ViewData["UserId"] = new SelectList(users, "UserId", "Email", ordersNamNh.UserId);

            var orders = await _orders.GetAllAsync();
            ViewData["order_id"] = new SelectList(orders, "OrderId", "OrderCode");
            return View(ordersNamNh);
        }


        // GET: OrdersNamNhs/Delete/5
       // [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ordersNamNh = await _orders.GetByIdAsync(id.Value);

            if (ordersNamNh == null)
            {
                return NotFound();
            }

            return View(ordersNamNh);
        }



        //POST: OrdersNamNhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        ///[Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var result = await _orders.DeleteAsync(id);

            if (result)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveDelete_OrdersNamNH", id);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Delete), new { id = id });

        }


    }
}
