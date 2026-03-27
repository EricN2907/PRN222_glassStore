using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using glassStore.Entites.NamNH.Models;
using glassStore.Service.NamNH.Interface;
using glassStore.Repositories.NamNH;
using Microsoft.AspNetCore.SignalR;
using glassStore.RazorWebApp.NamNH.Hubs;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class IndexModel : PageModel
    {
        private readonly IOrdersNamNhService _service;
        private readonly Order_Detail_NamNHRepositories _serviceSub;
        private readonly IHubContext<glassStore_Hub> _hubContext;

        public IndexModel(IOrdersNamNhService service, Order_Detail_NamNHRepositories repo, IHubContext<glassStore_Hub> hubContext)
        {
            _service = service;
            _serviceSub = repo;
            _hubContext = hubContext;
        }

        [BindProperty(SupportsGet = true)]
        public string? OrderCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PhoneNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReceiverNameSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public List<OrdersNamNh> OrdersNamNh { get ; set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 10;
            OrdersNamNh = await _service.SearchAsync(OrderCode, PhoneNumber, ReceiverNameSearch, PageNumber, pageSize);
            var totalItems = await _service.GetSearchCountAsync(OrderCode, PhoneNumber, ReceiverNameSearch);
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveDelete_OrdersNamNH", id);
                return new JsonResult(new { success = true });
            }
            return new JsonResult(new { success = false, message = "Delete failed" });
        }
    }
}
