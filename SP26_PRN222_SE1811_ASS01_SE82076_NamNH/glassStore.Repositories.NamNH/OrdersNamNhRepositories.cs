using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using glassStore.Entites.NamNH.Models;
using glassStore_Repositories.NamNH.Base;
using Microsoft.EntityFrameworkCore;

// class main table 
namespace glassStore.Repositories.NamNH
{
    public class OrdersNamNhRepositories : GenericRepository<OrdersNamNh>
    {
        public OrdersNamNhRepositories(){ }

        public OrdersNamNhRepositories(glass_StoreContext context) => _context = context;
      
        public async Task<List<OrdersNamNh>> GetAllAsync()
        {
            var items = await _context.OrdersNamNhs
                              .Include(o => o.OrderDetailNamNhs)
                .ToListAsync();
            return items ?? new List<OrdersNamNh>();
        }

        public async Task<OrdersNamNh> GetById(int order_id)
        {
            var item = await _context.OrdersNamNhs
                .Include(o => o.OrderDetailNamNhs)
                .FirstOrDefaultAsync(c => c.OrderId == order_id);

            return item ?? new OrdersNamNh();
        }

        public async Task<List<OrdersNamNh>> SearchAsync(string order_code, string phone_number, string product_name)
        {
            return await _context.OrdersNamNhs
                    .Include(o => o.OrderDetailNamNhs) 
                    .Where(c =>
                        (string.IsNullOrEmpty(order_code) || c.OrderCode.Contains(order_code)) &&
                        (string.IsNullOrEmpty(phone_number) || c.ReceiverPhone.Contains(phone_number)) &&
                        (string.IsNullOrEmpty(product_name) || c.OrderDetailNamNhs.Any(d => d.ProductNameSnapshot.Contains(product_name)))
                    )
                    .OrderByDescending(c => c.CreatedAt) 
                    .ToListAsync();
        }

    }
}
