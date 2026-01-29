using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using glassStore.Entites.NamNH.Models;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class DeleteModel : PageModel
    {
        private readonly glassStore.Entites.NamNH.Models.glass_StoreContext _context;

        public DeleteModel(glassStore.Entites.NamNH.Models.glass_StoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrdersNamNh OrdersNamNh { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersnamnh = await _context.OrdersNamNhs.FirstOrDefaultAsync(m => m.OrderId == id);

            if (ordersnamnh == null)
            {
                return NotFound();
            }
            else
            {
                OrdersNamNh = ordersnamnh;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersnamnh = await _context.OrdersNamNhs.FindAsync(id);
            if (ordersnamnh != null)
            {
                OrdersNamNh = ordersnamnh;
                _context.OrdersNamNhs.Remove(OrdersNamNh);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
