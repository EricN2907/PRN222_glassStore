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
    public class SystemUserAccountRepositories : GenericRepository<SystemUserAccount>
    {
        public SystemUserAccountRepositories() { }  
        public SystemUserAccountRepositories(glass_StoreContext context) => _context = context;

        public async Task<SystemUserAccount> GetUserAsync(string user_name, string password)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync(c => c.Email == user_name && c.Password == password)
                 ?? new SystemUserAccount();

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync(c => c.UserName == user_name && c.Password == password)
            //     ?? new SystemUserAccount();

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync(c => c.UserName == user_name && c.Password == password)
            //     ?? new SystemUserAccount();

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync(c => c.UserName == user_name && c.Password == password)
            //     ?? new SystemUserAccount();
        }
    }
}
    