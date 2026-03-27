using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using glassStore.RazorWebApp.NamNH.Hubs;
using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH;
using glassStore.Service.NamNH.Interface;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class DeleteModel : PageModel
    {

        private readonly IOrdersNamNhService _service;
        private readonly OrderDetailNamNhService _details;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public DeleteModel(IOrdersNamNhService service, OrderDetailNamNhService detail, IHubContext<glassStore_Hub> hubContext)
        {
            _service = service;
            _details = detail;
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

            //var ordersnamnh = await _context.OrdersNamNhs.FirstOrDefaultAsync(m => m.OrderId == id);
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            OrdersNamNh = result;
            return Page();  
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("ReceiveDelete_OrdersNamNH", id.Value);
                return RedirectToPage("./Index");
            }
        }
    }
}
