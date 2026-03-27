using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;

namespace glassStore.Service.NamNH.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<SystemUserAccount?> LoginAsync(string email, string password);
    }
}
