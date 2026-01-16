using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;
using glassStore.Service.NamNH.Interface;

namespace glassStore.Service.NamNH
{
    public class OrdersNamNhService : IOrdersNamNhService
    {
        private readonly OrdersNamNhRepositories _repo;

        public OrdersNamNhService() => _repo ??= new OrdersNamNhRepositories();

        public async Task<int> CreateAsync(OrdersNamNh orders)
        {
            try
            {
                return await _repo.CreateAsync(orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public async Task<OrdersNamNh> GetByIdAsync(int id)
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

        public async Task<List<OrdersNamNh>> SearchAsync(string order_code, string phone_number, string product_name)
        {
            //throw new NotImplementedException();
            try
            {
                return await _repo.SearchAsync(order_code, phone_number, product_name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _repo.GetByIdAsync(id);
                if (item != null) {
                    return await _repo.RemoveAsync(item);
                }
                return false;
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
    }
}
