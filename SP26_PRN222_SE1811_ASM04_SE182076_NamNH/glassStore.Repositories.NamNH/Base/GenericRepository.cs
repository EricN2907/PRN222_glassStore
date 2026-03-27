using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using glassStore.Entites.NamNH.Models;
using glassStore_Repositories.NamNH;
using Microsoft.EntityFrameworkCore;

namespace glassStore_Repositories.NamNH.Base
{
    public class GenericRepository<T> where T : class
    {
        protected glass_StoreContext _context;

        public GenericRepository()
        {
            _context ??= new glass_StoreContext();
        }

        public GenericRepository(glass_StoreContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }
        
        public void Update(T entity)
        {
            //// Turning off Tracking for UpdateAsync in Entity Framework
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            try
            {
                // Simple attempt: Clear and Update
                _context.ChangeTracker.Clear();
                _context.Set<T>().Update(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Direct approach if tracking is extremely stubborn
                _context.ChangeTracker.Clear();
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }
        public async Task<T> GetByIdAsync(string code)
        {

            return await _context.Set<T>().FindAsync(code);
         }

    /*
    https://guidgenerator.com/
    uniqueidentifier | guid: daacb4fb-ff73-46ef-98f1-4af9aab2a30a
     */
    public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public void ClearTracker()
        {
            _context.ChangeTracker.Clear();
        }

        #region Separating asigned entity and save operators        

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            _context.ChangeTracker.Clear();
            _context.Set<T>().Update(entity);
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        #endregion Separating asign entity and save operators
    }
}
