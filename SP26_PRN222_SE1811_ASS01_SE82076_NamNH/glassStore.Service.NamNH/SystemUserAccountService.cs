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
    public class SystemUserAccountService : IAccountService
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
        public async Task<SystemUserAccount?> LoginAsync(string email, string password)
        {
            var user = await _systemUserAccountRepositories.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null; 
            }
            if(user.Password != password)
            {
                throw new Exception("INCORRECT_PASSWORD");
                //văng lỗi
            }
            if (user.IsActive == false)
            {
                throw new Exception("ACCOUNT_LOCKED");
                //văng lỗi
            }
           
            return user;
        }
    }
}
