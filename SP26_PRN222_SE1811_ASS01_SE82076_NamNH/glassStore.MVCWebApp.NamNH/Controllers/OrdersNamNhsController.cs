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

namespace glassStore.MVCWebApp.NamNH.Controllers
{
    public class OrdersNamNhsController : Controller
    {
        //private readonly glass_StoreContext _context;
        private readonly IOrdersNamNhService _orders;
        private readonly OrderDetailNamNhService _orderDetails;
        //public OrdersNamNhsController(glass_StoreContext context)
        //{
        //    _context = context;
        //}
        public OrdersNamNhsController(IOrdersNamNhService ordersNamNhService, OrderDetailNamNhService orderDetailNamNh) 
            {
            _orders ??= new OrdersNamNhService();
            _orderDetails ??= new OrderDetailNamNhService();
            }
    

        // GET: OrdersNamNhs
        public async Task<IActionResult> Index()
        {
            var item = await _orders.GetAllAsync();
            return item  == null ? NotFound() : View(item);
        }
        // GET: OrdersNamNhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            if (id == null) return NotFound();
            var order = await _orders.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: OrdersNamNhs/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            //ViewData["VoucherId"] = new SelectList(_context.VouchersTanTms, "VoucherId", "Code");
            //return View();
            // Gọi Service lấy data cho dropdown
            var item = await _orders.GetAllAsync();
            ViewData["order_id"] = new SelectList(item, "OrderId", "OrderCode");
            return View();
        }

        // POST: OrdersNamNhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind
            ("OrderId,UserId,VoucherId,OrderType,OrderCode,PaymentMethod,Subtotal,DiscountTotal,TaxTotal,ShippingFee,GrandTotal,Status,CustomerNote,ReceiverName,ReceiverPhone,ReceiverAddress,CreatedAt,UpdatedAt")] OrdersNamNh ordersNamNh)
        {
            if (ModelState.IsValid)
            {
                var result = await _orders.CreateAsync(ordersNamNh);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Create order failed.");
                }
                var orderDetails = await _orderDetails.GetAllAsync();
                ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode");
                return View(ordersNamNh);
            }
            return View(ordersNamNh);
        }

        // GET: OrdersNamNhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ordersNamNh = await _orders.GetByIdAsync(id.Value);
            if (ordersNamNh == null) return NotFound();

            // Đã bỏ code load ViewBag/ViewData tại đây
            return View(ordersNamNh);
        }

        // POST: OrdersNamNhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,VoucherId,OrderType,OrderCode,PaymentMethod,Subtotal,DiscountTotal,TaxTotal,ShippingFee,GrandTotal,Status,CustomerNote,ReceiverName,ReceiverPhone,ReceiverAddress,CreatedAt,UpdatedAt")] OrdersNamNh ordersNamNh)
        {
           
            if (id != ordersNamNh.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orders.UpdateAsync(ordersNamNh);
                }
                catch (Exception) // Bắt lỗi nếu Update thất bại
                {
                    // THAY THẾ logic Exists cũ bằng GetByIdAsync
                    var checkItem = await _orders.GetByIdAsync(id);

                    if (checkItem == null)
                    {
                        // Nếu tìm không thấy -> Trả về NotFound
                        return NotFound();
                    }
                    else
                    {
                        // Nếu tìm thấy mà vẫn lỗi -> Ném lỗi ra (do lỗi khác)
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ordersNamNh);
        }

        //// GET: OrdersNamNhs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ordersNamNh = await _context.OrdersNamNhs
        //        .Include(o => o.User)
        //        .Include(o => o.Voucher)
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (ordersNamNh == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ordersNamNh);
        //}

        // POST: OrdersNamNhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var ordersNamNh = await _context.OrdersNamNhs.FindAsync(id);
            //if (ordersNamNh != null)
            //{
            //    _context.OrdersNamNhs.Remove(ordersNamNh);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            await _orders.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

      
    }
}
