using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH.Interface;
using Microsoft.EntityFrameworkCore;

namespace glassStore.Service.NamNH
{
    public class OrdersNamNhService : IOrdersNamNhService
    {
        private readonly OrdersNamNhRepositories _repo;

        public OrdersNamNhService(OrdersNamNhRepositories repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(OrdersNamNh orders)
        {
            try
            {
                return await _repo.CreateAsync(orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<OrdersNamNh>> GetAllAsync()
        {
            //throw new NotImplementedException();
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrdersNamNh> GetByIdAsync(int? id)
        {
            //throw new NotImplementedException();
            try
            {
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrdersNamNh>> SearchAsync(string order_code, string phone_number, string receiver_name, int pageNumber = 1, int pageSize = 10)
        {
            //throw new NotImplementedException();
            try
            {
                return await _repo.SearchAsync(order_code, phone_number, receiver_name, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetSearchCountAsync(string order_code, string phone_number, string receiver_name)
        {
            try
            {
                return await _repo.GetSearchCountAsync(order_code, phone_number, receiver_name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            try
            {
                if (id == null) return false;
                
                // Use ExecuteDeleteAsync to bypass tracking issues entirely (available in EF Core 7+)
                var rowsAffected = await _repo.GetContext().OrdersNamNhs
                    .Where(o => o.OrderId == id)
                    .ExecuteDeleteAsync();
                
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
        public async Task<int> UpdateAsync(OrdersNamNh orders)
        {
            try { 
                return await _repo.UpdateAsync(orders);
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item != null;
        }
    }
}
