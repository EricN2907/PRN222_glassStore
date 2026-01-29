using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using glassStore.Entites.NamNH.Models;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class EditModel : PageModel
    {
        private readonly glassStore.Entites.NamNH.Models.glass_StoreContext _context;

        public EditModel(glassStore.Entites.NamNH.Models.glass_StoreContext context)
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

            var ordersnamnh =  await _context.OrdersNamNhs.FirstOrDefaultAsync(m => m.OrderId == id);
            if (ordersnamnh == null)
            {
                return NotFound();
            }
            OrdersNamNh = ordersnamnh;
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
           ViewData["VoucherId"] = new SelectList(_context.VouchersTanTms, "VoucherId", "Code");
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

            _context.Attach(OrdersNamNh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersNamNhExists(OrdersNamNh.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrdersNamNhExists(int id)
        {
            return _context.OrdersNamNhs.Any(e => e.OrderId == id);
        }
    }
}
