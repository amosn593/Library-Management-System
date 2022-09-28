using DAL.Data;
using DOMAIN.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly LibraryDbContext _context;
        public GenericRepository(LibraryDbContext context)
        {
            _context = context;
        }

        
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            
            try
            {
                return await _context.Set<T>().ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public virtual async Task<T> GetById(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                return entity;

            }
            catch (Exception)
            {
                throw;
            }


        }

        public virtual void Add(T entity)
        {
            try
            {
                 _context.Set<T>().Add(entity);
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task Delete(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                _context.Set<T>().Remove(entity);
                

            }
            catch (Exception)
            {
                throw;
            }


        }

        public virtual void Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);

            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
