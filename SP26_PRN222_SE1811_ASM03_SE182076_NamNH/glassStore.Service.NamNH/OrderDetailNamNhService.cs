using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;

namespace glassStore.Service.NamNH
{
    public class OrderDetailNamNhService
    {
        private readonly Order_Detail_NamNHRepositories _repo;

        public OrderDetailNamNhService() => _repo ??= new Order_Detail_NamNHRepositories();

        public async Task<List<OrderDetailNamNh>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
