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

        public UserService() => _repo ??= new UserRepositories();

        public async Task<List<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
