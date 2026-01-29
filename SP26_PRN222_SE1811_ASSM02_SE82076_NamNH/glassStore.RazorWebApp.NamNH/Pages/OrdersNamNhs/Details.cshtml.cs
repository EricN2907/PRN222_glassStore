using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class DetailsModel : PageModel
    {
        private readonly IOrdersNamNhService _service;
        private readonly Order_Detail_NamNHRepositories _serviceSub;

        public DetailsModel(IOrdersNamNhService service, Order_Detail_NamNHRepositories repo)
        {
            _service = service;
            _serviceSub = repo;
        }

        public OrdersNamNh OrdersNamNh { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersnamnh = await _service.GetByIdAsync(id);
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
    }
}
