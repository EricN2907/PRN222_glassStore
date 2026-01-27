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
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Index(string order_code, string phone_number, string product_name)
        {
            var items = await _orders.SearchAsync(order_code,phone_number,product_name);
            return View(items);
        }
        [Authorize(Roles = "1, 2")]
        // GET: OrdersNamNhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            if (id == null) return NotFound();
            var order = await _orders.GetByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }
        [Authorize(Roles = "1, 2")]
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
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Create(OrdersNamNh ordersNamNh)
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

        [Authorize(Roles = "1")]
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
            return View(item);
        }


        //POST: OrdersNamNhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")] 
        public async Task<IActionResult> Edit(int id, OrdersNamNh ordersNamNh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _orders.UpdateAsync(ordersNamNh);
                    if(result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }
            var orders = await _orders.GetAllAsync();
            ViewData["order_id"] = new SelectList(orders, "OrderId", "OrderCode");
            return View(ordersNamNh);
        }


        // GET: OrdersNamNhs/Delete/5
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var result = await _orders.DeleteAsync(id);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Delete), new { id = id });

        }


    }
}
