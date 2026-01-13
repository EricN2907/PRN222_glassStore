using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore.Repositories.NamNH;

namespace glassStore.Service.NamNH
{
    public class SystemUserAccountService 
    {
        private readonly SystemUserAccountRepositories _systemUserAccountRepositories;

        public SystemUserAccountService() => _systemUserAccountRepositories ??= new SystemUserAccountRepositories();

        public async Task<SystemUserAccount> GetUserAsync(string user_name, string password)
        {
            try
            {
                return await _systemUserAccountRepositories.GetUserAsync(user_name, password);
            }
            catch
            {
                throw new Exception("Error retrieving user.");
            }
        }
    }
}
