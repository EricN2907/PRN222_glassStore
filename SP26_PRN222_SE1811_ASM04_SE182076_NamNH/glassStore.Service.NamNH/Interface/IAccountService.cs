using glassStore.Entites.NamNH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glassStore.Service.NamNH.Interface
{
    public interface IAccountService
    {
        Task<SystemUserAccount> GetUserAsync(string user_name, string password);
        Task<SystemUserAccount?> LoginAsync(string email, string password);
    }
}
