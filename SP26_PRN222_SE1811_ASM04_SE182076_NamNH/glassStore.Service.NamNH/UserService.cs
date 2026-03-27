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
    public class UserService : IUserService
    {
        private readonly UserRepositories _repo;
 
        public UserService(UserRepositories repo)
        {
            _repo = repo;
        }
 
        public async Task<List<User>> GetAllAsync()
        {
            return await _repo.GetAllUsersAsync();
        }
 
        public async Task<SystemUserAccount?> LoginAsync(string email, string password)
        {
            var users = await _repo.GetAllSystemAccountsAsync();
            var trimmedEmail = email.Trim();
            var trimmedPassword = password.Trim();

            return users.FirstOrDefault(u => 
                (u.Email.Trim().Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase) || 
                 u.UserName.Trim().Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase)) && 
                u.Password.Trim() == trimmedPassword);
        }
    }
}
