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

namespace glassStore.RazorWebApp.NamNH.Pages.OrdersNamNhs
{
    public class IndexModel : PageModel
    {
        //private readonly glassStore.Entites.NamNH.Models.glass_StoreContext _context;
        private readonly IOrdersNamNhService _service;
        private readonly Order_Detail_NamNHRepositories _serviceSub;
        //public IndexModel(glassStore.Entites.NamNH.Models.glass_StoreContext context)
        //{
        //    _context = context;
        //}

        public IndexModel(IOrdersNamNhService service, Order_Detail_NamNHRepositories repo)
        {
            _service = service;
            _serviceSub = repo;
        }
        public List<OrdersNamNh> OrdersNamNh { get ; set; } = default!;

        public async Task OnGetAsync()
        {
            //OrdersNamNh = await _context.OrdersNamNhs
            //    .Include(o => o.User)
            //    .Include(o => o.Voucher).ToListAsync();
            OrdersNamNh = await _service.GetAllAsync();
        }
    }
}
