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
        
        public glass_StoreContext GetContext() => _context;
      
        public async Task<List<OrdersNamNh>> GetAllAsync()
        {
            var items = await _context.OrdersNamNhs
                              .Include(o => o.OrderDetailNamNhs)
                              .Include(o => o.User)
                .ToListAsync();
            return items ?? new List<OrdersNamNh>();
        }

        public async Task<OrdersNamNh> GetByIdAsync(int? order_id)
        {
            var item = await _context.OrdersNamNhs
                .Include(o => o.OrderDetailNamNhs)
                .Include(o => o.User)
                .FirstOrDefaultAsync(c => c.OrderId == order_id);

            return item;
        }

        public async Task<List<OrdersNamNh>> SearchAsync(string order_code, string phone_number, string receiver_name, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.OrdersNamNhs
                    .Include(o => o.OrderDetailNamNhs) 
                    .Include(o => o.User)
                    .Where(c =>
                        (string.IsNullOrEmpty(order_code) || (c.OrderCode != null && c.OrderCode.Contains(order_code))) &&
                        (string.IsNullOrEmpty(phone_number) || (c.ReceiverPhone != null && c.ReceiverPhone.Contains(phone_number))) &&
                        (string.IsNullOrEmpty(receiver_name) || (c.ReceiverName != null && c.ReceiverName.Contains(receiver_name)))
                    );

            return await query
                    .OrderByDescending(c => c.CreatedAt) 
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task<int> GetSearchCountAsync(string order_code, string phone_number, string receiver_name)
        {
            return await _context.OrdersNamNhs
                    .CountAsync(c =>
                        (string.IsNullOrEmpty(order_code) || (c.OrderCode != null && c.OrderCode.Contains(order_code))) &&
                        (string.IsNullOrEmpty(phone_number) || (c.ReceiverPhone != null && c.ReceiverPhone.Contains(phone_number))) &&
                        (string.IsNullOrEmpty(receiver_name) || (c.ReceiverName != null && c.ReceiverName.Contains(receiver_name)))
                    );
        }

    }
}
