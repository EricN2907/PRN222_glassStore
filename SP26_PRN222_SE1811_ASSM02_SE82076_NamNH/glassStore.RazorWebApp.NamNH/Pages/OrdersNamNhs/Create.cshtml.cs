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
        private readonly Order_Detail_NamNHRepositories _serviceSub;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public CreateModel(IOrdersNamNhService service, Order_Detail_NamNHRepositories repo, IHubContext<glassStore_Hub> hubContext)
        {
            _service = service;
            _serviceSub = repo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGet()
        {
            var item = await _service.GetAllAsync();
            ViewData["order_id"] = new SelectList(item, "OrderId", "OrderCode");

            //ViewData["VoucherId"] = new SelectList(_context.VouchersTanTms, "VoucherId", "Code");
            return Page();
        }

        [BindProperty]
        public OrdersNamNh OrdersNamNh { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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
