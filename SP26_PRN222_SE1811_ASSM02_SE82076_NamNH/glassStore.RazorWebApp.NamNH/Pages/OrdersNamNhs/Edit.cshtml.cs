using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly OrderDetailNamNhService _detail;
        public EditModel(IOrdersNamNhService service, OrderDetailNamNhService detail)
        {
            _service = service;
            _detail = detail;
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
            var orderDetails = await _detail.GetAllAsync();
            ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode");
            //chỗ này nên theo kiểu : 
            //            ViewData["order_id"] = new SelectList(orderDetails, "OrderId", "OrderCode","Bảng phụ hoặc user");

            //ViewData["VoucherId"] = new SelectList(_context.VouchersTanTms, "VoucherId", "Code");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var result = await _service.UpdateAsync(OrdersNamNh);
                if(result > 0)
                {
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
