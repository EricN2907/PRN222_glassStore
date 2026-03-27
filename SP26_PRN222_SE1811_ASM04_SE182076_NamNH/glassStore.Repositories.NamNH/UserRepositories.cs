using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore_Repositories.NamNH.Base;
using Microsoft.EntityFrameworkCore;

namespace glassStore.Repositories.NamNH
{
    public class UserRepositories : GenericRepository<User>
    {
        public UserRepositories() { }
        public UserRepositories(glass_StoreContext context) => _context = context;
 
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<SystemUserAccount>> GetAllSystemAccountsAsync()
        {
            return await _context.SystemUserAccounts.ToListAsync();
        }
    }
}
