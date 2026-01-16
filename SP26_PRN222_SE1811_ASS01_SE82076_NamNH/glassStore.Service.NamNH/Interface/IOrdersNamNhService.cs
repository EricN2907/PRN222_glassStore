using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;

namespace glassStore.Service.NamNH.Interface
{
    public interface IOrdersNamNhService
    {
        Task<List<OrdersNamNh>> GetAllAsync();
        // lưu ý ở id là theo cái data type của bảng mình cần
        Task<OrdersNamNh> GetByIdAsync(int id);
        //Task<OrdersNamNh> SearchAsync(string order_code, string amount, string bankNo);
        Task<List<OrdersNamNh>> SearchAsync(string order_code, string phone_number, string product_name);
        //
        Task<int> CreateAsync(OrdersNamNh orders);
        //
        Task<int> UpdateAsync(OrdersNamNh orders);
        //
        Task<bool> DeleteAsync(int id);
    }
}
