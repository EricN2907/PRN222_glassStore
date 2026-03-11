using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore_Repositories.NamNH.Base;

namespace glassStore.Repositories.NamNH
{
    public class Order_Detail_NamNHRepositories : GenericRepository<OrderDetailNamNh>
    {
        public Order_Detail_NamNHRepositories() { }
        public Order_Detail_NamNHRepositories(glass_StoreContext context) => _context = context;

         
    }
}
